using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;


namespace JFramework
{
    public class JSocketListener : IJSocketListener
    {
        public event Action<IJSocketListener> onListening;
        public event Action<IJSocketListener, SocketStatusCodes, string> onClosed;
        public event Action<IJSocketListener, string> onError;
        public event Action<IJSocketListener, string, byte[]> onBinary;

        TcpListener listener;
        readonly ConcurrentDictionary<string, TcpClient> clients = new ConcurrentDictionary<string, TcpClient>();
        // 新增字段
        //readonly ConcurrentDictionary<string, string> tokenToAccountId = new ConcurrentDictionary<string, string>();

        CancellationTokenSource internalCts;
        bool isListening;

        public object Clone()
        {
            var util = new Utility();
            return util.DeepClone(this, true);
        }

        /// <summary>
        /// 广播数据到所有已连接客户端（非阻塞式错误处理，通过 onError 回调）
        /// </summary>
        public void Send(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            foreach (var kv in clients)
            {
                var client = kv.Value;
                if (client == null || !client.Connected) continue;

                try
                {
                    var stream = client.GetStream();
                    lock (stream)
                    {
                        stream.Write(data, 0, data.Length);
                        stream.Flush();
                    }
                }
                catch (Exception ex)
                {
                    onError?.Invoke(this, $"Send to client {kv.Key} failed: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 发送数据到指定客户端，返回是否发送成功
        /// </summary>
        public bool Send(string clientId, byte[] data)
        {
            if (clientId == null) throw new ArgumentNullException(nameof(clientId));
            if (data == null) throw new ArgumentNullException(nameof(data));

            TcpClient client;
            if (!clients.TryGetValue(clientId, out client) || client == null || !client.Connected)
            {
                onError?.Invoke(this, $"SendToClient failed: client {clientId} not found or disconnected");
                return false;
            }

            try
            {
                var stream = client.GetStream();
                lock (stream)
                {
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                }
                return true;
            }
            catch (Exception ex)
            {
                onError?.Invoke(this, $"SendToClient {clientId} error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 启动监听。接收到字节块时触发 onBinary(this, data)。
        /// </summary>
        public void StartListening(ushort port, CancellationToken stoppingToken)
        {
            if (isListening) return;

            internalCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
            var token = internalCts.Token;

            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                isListening = true;
                onListening?.Invoke(this);

                Task.Run(async () =>
                {
                    try
                    {
                        while (!token.IsCancellationRequested)
                        {
                            TcpClient client = null;
                            try
                            {
                                var acceptTask = listener.AcceptTcpClientAsync();
                                var completed = await Task.WhenAny(acceptTask, Task.Delay(-1, token)).ConfigureAwait(false);
                                if (completed != acceptTask) break; // 取消
                                client = acceptTask.Result;
                            }
                            catch (OperationCanceledException)
                            {
                                break;
                            }
                            catch (Exception ex)
                            {
                                onError?.Invoke(this, $"Accept error: {ex.Message}");
                                await Task.Delay(100, token).ConfigureAwait(false);
                                continue;
                            }

                            if (client == null) continue;
                            var clientId = Guid.NewGuid().ToString("N");
                            clients.TryAdd(clientId, client);

                            // 后台读取客户端数据
                            _ = Task.Run(() => HandleClientAsync(clientId, client, token), token);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // 正常取消
                    }
                    catch (Exception ex)
                    {
                        onError?.Invoke(this, $"Listener error: {ex.Message}");
                    }
                    finally
                    {
                        StopListeningInternal(SocketStatusCodes.NormalClosure, "Stopped listening");
                    }
                }, token);
            }
            catch (Exception ex)
            {
                onError?.Invoke(this, $"StartListening failed: {ex.Message}");
                StopListeningInternal(SocketStatusCodes.NormalClosure, $"Start failed: {ex.Message}");
            }
        }

        async Task HandleClientAsync(string clientId, TcpClient client, CancellationToken token)
        {
            var stream = client.GetStream();
            var buffer = new byte[4096];

            try
            {
                while (!token.IsCancellationRequested && client.Connected)
                {
                    int bytesRead = 0;
                    try
                    {
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (IOException)
                    {
                        // 网络错误或对端断开
                        break;
                    }
                    catch (Exception ex)
                    {
                        onError?.Invoke(this, $"Client read error: {ex.Message}");
                        break;
                    }

                    if (bytesRead == 0) break; // 客户端关闭连接

                    var data = new byte[bytesRead];
                    Array.Copy(buffer, 0, data, 0, bytesRead);

                    try
                    {
                        onBinary?.Invoke(this, clientId, data);
                    }
                    catch (Exception ex)
                    {
                        onError?.Invoke(this, $"onBinary handler error: {ex.Message}");
                    }
                }
            }
            finally
            {
                TcpClient removed;
                clients.TryRemove(clientId, out removed);
                try { client.Close(); } catch { }
            }
        }

        /// <summary>
        /// 停止监听并关闭所有客户端
        /// </summary>
        public void StopListening()
        {
            StopListeningInternal(SocketStatusCodes.NormalClosure, "Stopped by user");
        }

        void StopListeningInternal(SocketStatusCodes code, string message)
        {
            if (!isListening && (internalCts == null || internalCts.IsCancellationRequested)) return;

            try
            {
                internalCts?.Cancel();
            }
            catch { }

            try
            {
                listener?.Stop();
            }
            catch (Exception ex)
            {
                onError?.Invoke(this, $"Stop listener error: {ex.Message}");
            }

            foreach (var kv in clients)
            {
                try
                {
                    kv.Value.Close();
                }
                catch { }
            }
            clients.Clear();

            isListening = false;
            onClosed?.Invoke(this, code, message);
        }
    }
}