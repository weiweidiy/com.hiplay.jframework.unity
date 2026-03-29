using System;
using System.Threading;
using System.Threading.Tasks;

namespace JFramework
{
    public interface IJNetworkServer
    {
        event Action onOpen;
        event Action<SocketStatusCodes, string> onClose;
        event Action<string, IJNetMessage> onMessage;
        //event Action<byte[]> onBinary;
        event Action<string> onError;
        IJSocketListener SocketListener { get; }

        Task StartListening(ushort port, CancellationToken stoppingToken);

        void Send(byte[] data);

        bool Send(string clientId, byte[] data);
    }
}