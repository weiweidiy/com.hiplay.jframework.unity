using UnityEngine;

namespace JFramework
{
    public interface IGameObjectPool
    {
        GameObject Rent(string location, Transform parent);

        void Return(GameObject obj);
    }
}

