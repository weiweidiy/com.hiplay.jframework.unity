using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace JFramework
{

    public interface ISocketListenerFactory
    {
        IJSocketListener Create();
    }

    public class JNetworkServer : IJNetworkServer
    {
        /// <summary>
        /// 接口事件
        /// </summary>
        public event Action onOpen;
        public event Action<SocketStatusCodes, string> onClose;
        public event Action<string, IJNetMessage> onMessage;
        public event Action<string> onError;

        /// <summary>
        /// socket对象
        /// </summary>
        IJSocketListener socketListener = null;
        public IJSocketListener SocketListener { get => socketListener; set => socketListener = value; }

        /// <summary>
        /// socket工厂
        /// </summary>
        ISocketListenerFactory socketFactory = null;

        /// <summary>
        /// 消息处理策略(加密，压缩等)
        /// </summary>
        INetworkMessageProcessStrate messageProcessStrate = null;

        /// <summary>
        /// 处理服务器消息的处理器，处理业务逻辑
        /// </summary>
        INetworkServerMessageHandler messageServerMessageHandler = null;

        /// <summary>
        /// token管理器，负责生成和验证token，维护token和accountId的映射关系
        /// </summary>
        ITokenManager tokenManager;

        /// <summary>
        /// 开始监听指定端口，等待客户端连接
        /// </summary>
        /// <param name="port"></param>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public Task StartListening(ushort port, CancellationToken stoppingToken)
        {
            InitSocketListener();
            socketListener.StartListening(port, stoppingToken); // 端口号可以从配置或参数中获取
            return Task.CompletedTask;
        }

        /// <summary>
        /// 创建socket
        /// </summary>
        /// <param name="url"></param>
        /// <param name="tcs"></param>
        /// <returns></returns>
        void InitSocketListener(/*ushort port*/)
        {
            socketListener = CreateSocket();
            //socketListener.Init(port);

            //监听事件
            socketListener.onListening += (s) => { Socket_OnOpen(s); };
            socketListener.onClosed += (s, code, message) => { Socket_OnClose(s, code, message); };
            socketListener.onBinary += async (s, clientId, data) => { await Socket_OnBinary(s, clientId, data); };
            //socketListener.onMessage += (s, message) => { Socket_OnMessage(s, message); };
            socketListener.onError += (s, message) => { Scoket_OnError(s, message); };
        }

        #region 响应事件
        public void Scoket_OnError(IJSocketListener s, string message)
        {
            onError?.Invoke(message);
        }


        /// <summary>
        /// 收到消息了
        /// </summary>
        /// <param name="s"></param>
        /// <param name="data"></param>
        public async Task Socket_OnBinary(IJSocketListener s, string clientId, byte[] data)
        {
            var message = GetNetworkMessageProcessStrate().ProcessComingMessage(data);

            //var token = message.Token;
            //if (!tokenManager.ValidateToken(token, out var accountId))
            //{
            //    //token无效，直接丢弃消息并返回错误响应
            //    var errorResponse = new JNetMessageError
            //    {
            //        ErrorMessage = "Invalid token"
            //    };
            //    var errorMsg = GetNetworkMessageProcessStrate().ProcessOutMessage(errorResponse);
            //    Send(clientId, errorMsg);
            //    return;

            //}

            //优先处理消息
            var responseMessage = await messageServerMessageHandler?.Handle(message);
            (responseMessage as JNetMessage).Uid = message.Uid; // 将请求消息的Uid原封不动地返回给客户端，以便客户端能够正确匹配响应

            var buffer = GetNetworkMessageProcessStrate().ProcessOutMessage(responseMessage);
            Send(clientId, buffer);

            onMessage?.Invoke(clientId, message);

        }

        /// <summary>
        /// 链接关闭了
        /// </summary>
        /// <param name="s"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public void Socket_OnClose(IJSocketListener s, SocketStatusCodes code, string message)
        {
            onClose?.Invoke(code, message);
        }

        /// <summary>
        /// 链接成功了
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="tcs"></param>
        public void Socket_OnOpen(IJSocketListener webSocket)
        {

            //在完成异步之后，再进行事件通知
            onOpen?.Invoke();
        }
        #endregion

        /// <summary>
        /// 获取socket
        /// </summary>
        /// <returns></returns>
        public IJSocketListener GetSocket() => socketListener;

        /// <summary>
        /// 创建socket
        /// </summary>
        /// <returns></returns>
        public IJSocketListener CreateSocket() => socketFactory.Create();

        /// <summary>
        /// 获取消息处理策略对象
        /// </summary>
        /// <returns></returns>
        public INetworkMessageProcessStrate GetNetworkMessageProcessStrate() => messageProcessStrate;

        /// <summary>
        /// 给所有客户端发送消息
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="Exception"></exception>
        public void Send(byte[] data)
        {
            var socket = GetSocket();
            if (socket == null)
                throw new Exception("Socket is not open or initialized.");

            socket.Send(data);
        }

        /// <summary>
        /// 给指定客户端发送消息
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool Send(string clientId, byte[] data)
        {
            var socket = GetSocket();
            if (socket == null)
                throw new Exception("Socket is not open or initialized.");

            return socket.Send(clientId, data);
        }

        public JNetworkServer(ISocketListenerFactory socketFactory, INetworkMessageProcessStrate messageProcessStrate, INetworkServerMessageHandler messageHandler,ITokenManager tokenManager)
        {
            this.socketFactory = socketFactory;
            this.messageProcessStrate = messageProcessStrate;
            this.messageServerMessageHandler = messageHandler;
            this.tokenManager = tokenManager;
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