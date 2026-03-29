using System;

namespace JFramework
{
    public interface IObjectPool
    {
        T Rent<T>(Action<T> onGet = null);

        void Return<T>(T obj);
    }
}