namespace JFramework
{
    public class JSocketListenerFactory : ISocketListenerFactory
    {
        IJSocketListener socket;

        /// <summary>
        /// 简单的socket工厂，使用原型模式创建socket实例
        /// </summary>
        public JSocketListenerFactory(IJSocketListener socket)
        {
            this.socket = socket;
        }
        public IJSocketListener Create()
        {
            return socket.Clone() as IJSocketListener;
        }
    }

    ///// <summary>
    ///// 简单的 TCP 服务器实现：
    ///// - 接受客户端连接
    ///// - 从流中读取原始字节，交给 INetworkMessageProcessStrate.ProcessComingMessage 解析
    ///// - 将解析到的 IJNetMessage 交给 INetworkMessageHandler.Handle 处理
    ///// - 提供 SendToClient 通过原始策略将 IJNetMessage 序列化并发送到指定客户端
    /////
    ///// 注意：此实现对 TCP 分包/粘包没有额外的消息边界处理（仅按到达的字节块交付）。
    ///// 若协议需要可靠的边界（推荐），请使用长度前缀或其它帧协议在消息处理策略中实现。
    ///// </summary>
    //public class JNetworkServer : IJNetworkServer, IDisposable
    //{
    //    readonly int port;
    //    readonly INetworkMessageProcessStrate messageProcessStrate;
    //    readonly INetworkMessageHandler messageHandler;

    //    TcpListener listener;
    //    readonly ConcurrentDictionary<string, TcpClient> clients = new ConcurrentDictionary<string, TcpClient>();
    //    bool disposed;

    //    public JNetworkServer(int port, INetworkMessageProcessStrate processStrate, INetworkMessageHandler handler)
    //    {
    //        this.port = port;
    //        this.messageProcessStrate = processStrate ?? throw new ArgumentNullException(nameof(processStrate));
    //        this.messageHandler = handler; // handler 可为 null（仅解析并丢弃/触发其它逻辑）
    //    }

    //    public async Task StartListening(CancellationToken stoppingToken)
    //    {
    //        if (disposed) throw new ObjectDisposedException(nameof(JNetworkServer));

    //        listener = new TcpListener(IPAddress.Any, port);
    //        listener.Start();

    //        try
    //        {
    //            while (!stoppingToken.IsCancellationRequested)
    //            {
    //                var acceptTask = listener.AcceptTcpClientAsync();
    //                var completed = await Task.WhenAny(acceptTask, Task.Delay(-1, stoppingToken)).ConfigureAwait(false);
    //                if (completed != acceptTask)
    //                    break; // cancellation

    //                TcpClient client = acceptTask.Result;
    //                var clientId = Guid.NewGuid().ToString("N");
    //                clients.TryAdd(clientId, client);

    //                // 后台处理客户端连接
    //                _ = Task.Run(() => HandleClientAsync(clientId, client, stoppingToken));
    //            }
    //        }
    //        catch (OperationCanceledException)
    //        {
    //            // 正常退出
    //        }
    //        finally
    //        {
    //            StopAllClients();
    //            listener?.Stop();
    //        }
    //    }

    //    async Task HandleClientAsync(string clientId, TcpClient client, CancellationToken stoppingToken)
    //    {
    //        var stream = client.GetStream();
    //        var buffer = new byte[4096];

    //        try
    //        {
    //            while (!stoppingToken.IsCancellationRequested && client.Connected)
    //            {
    //                int bytesRead = 0;
    //                try
    //                {
    //                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, stoppingToken).ConfigureAwait(false);
    //                }
    //                catch (OperationCanceledException)
    //                {
    //                    break;
    //                }

    //                if (bytesRead == 0)
    //                    break; // 客户端关闭连接

    //                var data = new byte[bytesRead];
    //                Array.Copy(buffer, 0, data, 0, bytesRead);

    //                // 交由策略解析消息
    //                IJNetMessage message = null;
    //                try
    //                {
    //                    message = messageProcessStrate.ProcessComingMessage(data);
    //                }
    //                catch (Exception ex)
    //                {
    //                    // 解析错误：记录或触发错误处理（此处简单继续）
    //                    System.Console.WriteLine($"[JNetworkServer] message parse error: {ex.Message}");
    //                    continue;
    //                }

    //                // 业务处理
    //                try
    //                {
    //                    messageHandler?.Handle(message);
    //                }
    //                catch (Exception ex)
    //                {
    //                    System.Console.WriteLine($"[JNetworkServer] message handler error: {ex.Message}");
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            System.Console.WriteLine($"[JNetworkServer] client error: {ex.Message}");
    //        }
    //        finally
    //        {
    //            TcpClient removed;
    //            clients.TryRemove(clientId, out removed);
    //            try { client.Close(); } catch { }
    //        }
    //    }

    //    /// <summary>
    //    /// 将消息发回指定客户端（返回是否发送成功）
    //    /// </summary>
    //    public bool SendToClient(string clientId, IJNetMessage message)
    //    {
    //        if (disposed) return false;
    //        if (message == null) throw new ArgumentNullException(nameof(message));

    //        TcpClient client;
    //        if (!clients.TryGetValue(clientId, out client) || client == null || !client.Connected)
    //            return false;

    //        try
    //        {
    //            var data = messageProcessStrate.ProcessOutMessage(message);
    //            var stream = client.GetStream();
    //            // 同步写入以简化逻辑；若高并发请改为异步并做好并发控制
    //            stream.Write(data, 0, data.Length);
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            System.Console.WriteLine($"[JNetworkServer] send error: {ex.Message}");
    //            return false;
    //        }
    //    }

    //    void StopAllClients()
    //    {
    //        foreach (var kv in clients)
    //        {
    //            try
    //            {
    //                kv.Value.Close();
    //            }
    //            catch { }
    //        }
    //        clients.Clear();
    //    }

    //    public void Dispose()
    //    {
    //        if (disposed) return;
    //        disposed = true;
    //        try
    //        {
    //            listener?.Stop();
    //        }
    //        catch { }
    //        StopAllClients();
    //    }
    //}
}