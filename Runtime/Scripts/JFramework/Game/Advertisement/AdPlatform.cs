using System;

namespace JFramework
{

    public abstract class AdPlatform : IAdPlatform
    {
        public abstract void Initialize(Action completed);
    }
}
