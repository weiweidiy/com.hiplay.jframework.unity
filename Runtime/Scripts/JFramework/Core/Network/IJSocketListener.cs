using System;
using System.Threading;

namespace JFramework
{
    public interface IJSocketListener  :  ICloneable
    {
        event Action<IJSocketListener> onListening;
        event Action<IJSocketListener, SocketStatusCodes, string> onClosed;
        event Action<IJSocketListener, string> onError;
        event Action<IJSocketListener, string, byte[]> onBinary;

        void StartListening(ushort port, CancellationToken stoppingToken);
        void StopListening();

        void Send(byte[] data);
        bool Send(string clientId, byte[] data);
    }
}
