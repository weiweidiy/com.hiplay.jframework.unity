using JFramework;

namespace JFramework.Unity
{
    public class DefaultSocketFactory : ISocketFactory
    {
        IJSocket socket;
        public DefaultSocketFactory(IJSocket socket)
        {
            this.socket = socket;
        }
        public IJSocket Create()
        {
            return socket.Clone() as IJSocket;
        }
    }


}
