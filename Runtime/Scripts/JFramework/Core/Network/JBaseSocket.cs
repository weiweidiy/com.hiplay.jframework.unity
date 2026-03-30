using System;
using System.Threading.Tasks;

namespace JFramework
{
    public abstract class JBaseSocket : IJSocket
    {
        public event Action<IJSocket> onOpen;
        protected void OnOpen(IJSocket socket)
        {
            onOpen?.Invoke(socket);
        }
        public event Action<IJSocket, string> onError;
        protected void OnError(IJSocket socket, string error)
        {
            onError?.Invoke(socket, error);
        }
        public event Action<IJSocket, byte[]> onBinary;
        protected void OnBinary(IJSocket socket, byte[] data)
        {
            onBinary?.Invoke(socket, data);
        }
        public event Action<IJSocket, string> onMessage;
        protected void OnMessage(IJSocket socket, string message)
        {
            onMessage?.Invoke(socket, message);
        }
        public event Action<IJSocket, SocketStatusCodes, string> onClosed;
        protected void OnClosed(IJSocket socket, SocketStatusCodes code, string reason)
        {
            onClosed?.Invoke(socket, code, reason);
        }

        public abstract bool IsOpen { get; set; }
        public abstract void Open();

        public abstract void Close();
        public abstract void Init(string url, string token = null);

        public virtual object Clone()
        {
            var util = new Utility();
            return util.DeepClone(this, true);
        }


        public abstract Task<byte[]> Send(byte[] data);

        public abstract Task<string> Send(string message);

        public abstract Task RPCVoid(string method, object param = null, TimeSpan? timeout = null);

        public abstract Task<TResponse> RPC<TResponse>(string method, object param = null, TimeSpan? timeout = null);

        //public abstract Task RPCVoid(string method, string param = null, TimeSpan? timeout = null);

        //public abstract Task<string> RPC(string method, string param = null, TimeSpan? timeout = null);
    }
}

