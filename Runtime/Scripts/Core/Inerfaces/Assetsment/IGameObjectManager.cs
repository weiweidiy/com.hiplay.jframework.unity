
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework.Unity
{
    public interface IGameObjectManager
    {
        UniTask PreloadGameObjects(List<string> prefabsList);
        GameObject Rent(string name, Transform parent);
        void Return(GameObject go);
        UniTask<GameObject> InstantiateAsync(string location, Transform parent);
    }
}
