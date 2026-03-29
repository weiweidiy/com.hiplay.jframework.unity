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

        Task Send(byte[] data);

        Task<T> Send<T>(byte[] data);

        Task Send(string message);

        Task<T> Send<T>(string message);
    }
}
