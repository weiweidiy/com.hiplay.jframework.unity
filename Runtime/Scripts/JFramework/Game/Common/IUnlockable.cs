using System;
using System.Collections.Generic;
using System.Text;

namespace JFramework.Game
{
    /// <summary>
    /// 可以被解锁
    /// </summary>
    public interface IUnlockable
    {
        void Unlock();

        void Lock();

        bool IsLocked();
    }

    public interface IUnlockableContainer
    {
        bool Unlock(string uid);

        bool Lock(string uid);

        bool IsLocked(string uid);
    }
}
