using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace JFramework
{
    public interface IGameObjectPool
    {
        UniTask Regist(string location, Transform root = null, int capacity = 10
                                , Action<GameObject> onCreate = null
                                , Action<GameObject> onRelease = null
                                , Action<GameObject> onRent = null
                                , Action<GameObject> onReturn = null);

        bool HasRegist(string location);

        GameObject Rent(string location, Transform parent);

        void Return(GameObject obj);
    }
}

