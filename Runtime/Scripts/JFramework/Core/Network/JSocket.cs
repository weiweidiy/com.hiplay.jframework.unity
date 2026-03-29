using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace JFramework
{
    public class JSocket : JBaseSocket
    {
        private Socket _socket;
        private string _url;
        private string _host;
        private int _port;
        private string _token;
        private bool _isOpen;
        private Thread _receiveThread;

        public override bool IsOpen { get => _isOpen; set => _isOpen = value; }

        public override void Init(string url, string token = null)
        {
            _url = url;
            _token = token;

            // 解析 host:port
            // 支持格式: "127.0.0.1:12345"
            var parts = url.Split(':');
            if (parts.Length != 2 || !int.TryParse(parts[1], out _port))
            {
                OnError(this, "Invalid url format. Use host:port");
                return;
            }
            _host = parts[0];
        }

        public override void Open()
        {
            try
            {
                if (_socket != null)
                {
                    Close();
                }

                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.Connect(_host, _port);
                _isOpen = true;
                OnOpen(this);

                // 启动接收线程
                _receiveThread = new Thread(ReceiveLoop);
                _receiveThread.IsBackground = true;
                _receiveThread.Start();
            }
            catch (Exception ex)
            {
                _isOpen = false;
                OnError(this, ex.Message);
            }
        }

        public override void Close()
        {
            try
            {
                _isOpen = false;
                _socket?.Shutdown(SocketShutdown.Both);
                _socket?.Close();
                _socket = null;
                OnClosed(this, SocketStatusCodes.NormalClosure, "Closed by user");
            }
            catch (Exception ex)
            {
                OnError(this, ex.Message);
            }
        }

        public override Task Send(byte[] data)
        {
            try
            {
                if (_socket != null && _isOpen)
                {
                    _socket.Send(data);
                    return Task.CompletedTask;
                }
                else
                {
                    OnError(this, "Socket is not open.");
                }
            }
            catch (Exception ex)
            {
                OnError(this, ex.Message);
            }

            return Task.CompletedTask;
        }

        private void ReceiveLoop()
        {
            var buffer = new byte[4096];
            try
            {
                while (_isOpen && _socket != null)
                {
                    int bytesRead = _socket.Receive(buffer);
                    if (bytesRead > 0)
                    {
                        var data = new byte[bytesRead];
                        Array.Copy(buffer, data, bytesRead);
                        OnBinary(this, data);
                    }
                    else
                    {
                        // 连接关闭
                        _isOpen = false;
                        OnClosed(this, SocketStatusCodes.NormalClosure, "Remote closed");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _isOpen = false;
                OnError(this, ex.Message);
                OnClosed(this, SocketStatusCodes.NormalClosure, "Receive error");
            }
        }

        public override async Task<TResponse> Send<TResponse>(byte[] data)
        {
            // 发送数据
            await Send(data);

            // 等待并接收响应（阻塞式，实际项目建议用更优雅的异步/事件回调机制）
            // 这里只做简单实现：同步接收一段数据并反序列化为T
            if (_socket != null && _isOpen)
            {
                var buffer = new byte[4096];
                int bytesRead = _socket.Receive(buffer);
                if (bytesRead > 0)
                {
                    var responseData = new byte[bytesRead];
                    Array.Copy(buffer, responseData, bytesRead);

                    // 假设返回的是字符串（如JSON），可根据实际需求调整
                    string responseStr = System.Text.Encoding.UTF8.GetString(responseData);
                    try
                    {
                        // 尝试反序列化为T
                        return (TResponse)Convert.ChangeType(responseStr, typeof(TResponse));
                    }
                    catch
                    {
                        // 如果T不是string，建议用Json反序列化
                        try
                        {
                            return Newtonsoft.Json.JsonConvert.DeserializeObject<TResponse>(responseStr);
                        }
                        catch
                        {
                            OnError(this, "Response cannot be converted to type " + typeof(TResponse).Name);
                        }
                    }
                }
            }
            return default;
        }

        public override Task Send(string message)
        {
            if (string.IsNullOrEmpty(message))
                return Task.CompletedTask;

            var data = System.Text.Encoding.UTF8.GetBytes(message);
            return Send(data);
        }

        public override async Task<TResponse> Send<TResponse>(string message)
        {
            var data = System.Text.Encoding.UTF8.GetBytes(message);
            return await Send<TResponse>(data);
        }
    }
}