using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
//using System.Reactive.Linq;
//using System.Reactive.Subjects;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JFramework
{
    public interface ISocketFactory
    {
        IJSocket Create();
    }
    public class JNetwork : IJNetwork
    {
        /// <summary>
        /// 接口事件
        /// </summary>
        public event Action onOpen;
        public event Action<SocketStatusCodes, string> onClose;
        public event Action<IJNetMessage> onMessage;
        public event Action<string> onError;

        /// <summary>
        /// socket对象
        /// </summary>
        IJSocket socket = null;
        public IJSocket Socket { get => socket; set => socket = value; }

        /// <summary>
        /// socket工厂
        /// </summary>
        ISocketFactory socketFactory = null;

        /// <summary>
        /// 任务管理器
        /// </summary>
        IJTaskCompletionSourceManager<IUnique> taskManager = null;

        /// <summary>
        /// 消息处理策略(加密，压缩等)
        /// </summary>
        INetworkMessageProcessStrate messageProcessStrate = null;

        /// <summary>
        /// 消息处理器，处理业务逻辑
        /// </summary>
        INetworkMessageHandler messageHandler = null;


        #region 公开接口

        /// <summary>
        /// 会在发送消息时，创建一个任务，并等待这个任务完成或者超时，任务完成的时机是在收到响应消息的时候，调用tcs.TrySetResult(message)来完成任务
        /// 适合websocket这种需要等待响应的通信方式，发送消息的时候，可以等待这个任务完成，拿到响应结果，或者超时
        /// </summary>
        /// <param name="taskManager"></param>
        public void UseTaskManager(IJTaskCompletionSourceManager<IUnique> taskManager)
        {
            this.taskManager = taskManager;
        }

        /// <summary>
        /// 发起连接，RPC调用风格，直接等待响应
        /// </summary>
        /// <param name="socketName"></param>
        /// <param name="url"></param>
        /// <param name="msgEncode"></param>
        /// <param name="msgDecode"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Connect(string url, string token = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            try
            {
                InitSocket(url, tcs, token);
                GetSocket().Open();
                await tcs.Task;
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
                throw;
            }
        }

        /// <summary>
        /// 关闭链接
        /// </summary>
        public void Disconnect()
        {
            var socket = GetSocket();
            try
            {
                if (socket == null || !socket.IsOpen)
                    return;


                socket.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 是否链接
        /// </summary>
        /// <returns></returns>
        public bool IsConnecting()
        {
            var socket = GetSocket();

            if (socket == null)
                return false;

            return socket.IsOpen;
        }

        /// <summary>
        /// 发送消息， RPC风格调用， 如果像protobuf这种无法实现接口的，可以自己定义个适配器，实现iunique接口即可
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="pMsg"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<TResponse> SendMessage<TResponse>(IJNetMessage pMsg, TimeSpan? timeout = null) where TResponse : class, IJNetMessage
        {
            var socket = GetSocket();

            if (socket == null || !socket.IsOpen)
                throw new Exception("链接已断开，无法发送消息 socket");

            //创建任务
            var taskManager = GetTaskManager();
            TaskCompletionSource<IUnique> tcs = null;
            if (taskManager != null)
            {
                tcs = taskManager.AddTask(pMsg.Uid);
                if (tcs == null)
                    throw new Exception("Duplicate UID detected.");
            }


            try
            {
                //等待任务完成或者超时
                if (taskManager != null)
                {
                    var result = await taskManager.WaitingTask(pMsg.Uid, timeout); //可能超时
                    return result as TResponse; // 等待直到 OnWebSocketMessage 调用 TrySetResult
                }
                else
                {
                    //处理消息
                    var byteMsg = GetNetworkMessageProcessStrate().ProcessOutMessage(pMsg);
                    //发送
                    var response = await socket.Send(byteMsg);

                    var message = GetNetworkMessageProcessStrate().ProcessComingMessage(response);

                    return message as TResponse;
                }
            }
            catch (Exception ex)
            {
                if(tcs != null)
                {
                    if (!tcs.Task.IsCompleted)
                    {
                        GetTaskManager().SetException(pMsg.Uid, ex);
                        throw;
                    }
                }

                throw;

            }
            finally
            {
                if(taskManager!=null)
                {
                    var result = GetTaskManager().RemoveTask(pMsg.Uid);
                }
                 
            }

        }
        #endregion

        #region 响应事件
        public void Scoket_OnError(IJSocket s, string message, TaskCompletionSource<bool> tcs)
        {
            tcs.SetException(new Exception(message));

            onError?.Invoke(message);
        }


        /// <summary>
        /// 收到消息了
        /// </summary>
        /// <param name="s"></param>
        /// <param name="data"></param>
        public void Socket_OnBinary(IJSocket s, byte[] data)
        {
            var message = GetNetworkMessageProcessStrate().ProcessComingMessage(data);

            try
            {
                //如果没有tcs，那可能是一个推送消息
                var tcs = GetTaskManager().GetTask(message.Uid);
                if (tcs != null)
                {
                    tcs.TrySetResult(message); // 完成等待的任务
                }
            }
            catch (Exception ex)
            {
                // 处理解析错误
                Console.WriteLine($"Error parsing message: {ex.Message}");
                throw;
            }

            //优先处理消息
            messageHandler?.Handle(message);
            onMessage?.Invoke(message);

        }

        /// <summary>
        /// 链接关闭了
        /// </summary>
        /// <param name="s"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public void Socket_OnClose(IJSocket s, SocketStatusCodes code, string message)
        {
            onClose?.Invoke(code, message);
        }

        /// <summary>
        /// 链接成功了
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="tcs"></param>
        public void Socket_OnOpen(IJSocket webSocket, TaskCompletionSource<bool> tcs)
        {
            tcs.SetResult(true);

            //在完成异步之后，再进行事件通知
            onOpen?.Invoke();
        }
        #endregion


        /// <summary>
        /// 创建socket
        /// </summary>
        /// <param name="url"></param>
        /// <param name="tcs"></param>
        /// <returns></returns>
        void InitSocket(string url, TaskCompletionSource<bool> tcs, string token)
        {
            socket = CreateSocket();
            socket.Init(url, token);

            //监听事件
            socket.onOpen += (s) => { Socket_OnOpen(s, tcs); };
            socket.onClosed += (s, code, message) => { Socket_OnClose(s, code, message); };
            socket.onBinary += (s, data) => { Socket_OnBinary(s, data); };
            //socketListener.onMessage += (s, message) => { Socket_OnMessage(s, message); };
            socket.onError += (s, message) => { Scoket_OnError(s, message, tcs); };
        }

        /// <summary>
        /// 获取socket
        /// </summary>
        /// <returns></returns>
        public IJSocket GetSocket() => socket;

        /// <summary>
        /// 创建socket
        /// </summary>
        /// <returns></returns>
        public IJSocket CreateSocket() => socketFactory.Create();

        /// <summary>
        /// 获取任务管理器
        /// </summary>
        /// <returns></returns>
        public IJTaskCompletionSourceManager<IUnique> GetTaskManager() => taskManager;

        /// <summary>
        /// 获取消息处理策略对象
        /// </summary>
        /// <returns></returns>
        public INetworkMessageProcessStrate GetNetworkMessageProcessStrate() => messageProcessStrate;



        public JNetwork(ISocketFactory socketFactory,  INetworkMessageProcessStrate messageProcessStrate, INetworkMessageHandler messageHandler)
        {
            this.socketFactory = socketFactory;
            //this.taskManager = taskManager;
            this.messageProcessStrate = messageProcessStrate;
            this.messageHandler = messageHandler;
        }
    }
}
