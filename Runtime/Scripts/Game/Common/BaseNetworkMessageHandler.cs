using System;

namespace JFramework.Unity
{
    public abstract class BaseNetworkMessageHandler : INetworkMessageHandler
    {
        public abstract void Handle(IJNetMessage message);
    }
}
