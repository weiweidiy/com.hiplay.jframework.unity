using System.Net.WebSockets;
using System;
using System.Threading.Tasks;


namespace JFramework
{
    public enum SocketStatusCodes
    {
        NormalClosure
    }

    public interface IJNetwork : IJNetworkable
    {
        event Action onOpen;
        event Action<SocketStatusCodes, string> onClose;
        event Action<IJNetMessage> onMessage;
        //event Action<byte[]> onBinary;
        event Action<string> onError;

        IJSocket Socket { get; }
    }

    /// <summary>
    /// 可链接通信的接口
    /// </summary>
    public interface IJNetworkable
    {
        Task Connect(string url, string token = null);

        void Disconnect();

        Task<TResponse> SendMessage<TResponse>(IJNetMessage pMsg, TimeSpan? timeout = null) where TResponse : class, IJNetMessage;

        Task<TResponse> RPC<TResponse>(string method, object param = null, TimeSpan? timeout = null);

        Task RPCVoid(string method, object param = null, TimeSpan? timeout = null);

        bool IsConnecting();

        
    }
}