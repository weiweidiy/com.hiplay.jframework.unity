using System;
using System.Threading;
using System.Threading.Tasks;

namespace JFramework
{
    public interface IJSocket :  ICloneable
    {
        event Action<IJSocket> onOpen;
        event Action<IJSocket, SocketStatusCodes, string> onClosed;
        event Action<IJSocket, string> onError;
        event Action<IJSocket, byte[]> onBinary;
        event Action<IJSocket, string> onMessage;


        bool IsOpen { get; }
        void Init(string url, string token = null);
        void Open();
        void Close();

        /// <summary>
        /// 普通发送数据，等待服务器返回响应数据（需要有uid标识请求和响应的对应关系）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<byte[]> Send(byte[] data);

        Task<string> Send(string message);

        /// <summary>
        /// RPC调用，发送一个方法名和参数，等待服务器返回结果
        /// </summary>
        /// <param name="method"></param>
        /// <param name="param"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task RPCVoid(string method, object param = null, TimeSpan? timeout = null);

        Task<TResponse> RPC<TResponse>(string method, object param = null, TimeSpan? timeout = null);
    }
}
