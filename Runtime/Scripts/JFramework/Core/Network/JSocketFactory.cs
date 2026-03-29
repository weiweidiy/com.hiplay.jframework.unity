
namespace JFramework
{

    public class JSocketFactory : ISocketFactory
    {
        IJSocket socket;

        /// <summary>
        /// 简单的socket工厂，使用原型模式创建socket实例
        /// </summary>
        public JSocketFactory(IJSocket socket)
        {
            this.socket = socket;
        }
        public IJSocket Create()
        {
            return socket.Clone() as IJSocket;
        }
    }


}
