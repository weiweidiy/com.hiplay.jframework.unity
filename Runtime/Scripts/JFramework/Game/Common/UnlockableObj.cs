using System;

namespace JFramework.Game
{
    [Serializable]
    public struct UnlockableObj : IUnique, IUnlockable
    {

        public string Uid { get; set; }

        public bool IsUnLocked { get; set; }

        public bool IsLocked() => !IsUnLocked;

        public void Lock() => IsUnLocked = false;

        public void Unlock() => IsUnLocked = true;
    }
}