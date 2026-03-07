using System;



namespace JFramework.Unity
{
    public abstract class BaseNetworkMessageHandler : INetworkMessageHandler
    {
        public IJFacade Facade { get; set; }

        public abstract void Handle(IJNetMessage message);
    }
}
    