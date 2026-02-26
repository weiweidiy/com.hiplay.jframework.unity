
using Cysharp.Threading.Tasks;
using JFramework;
using JFramework.Game;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace JFramework.Unity
{
    public class DefaultGameObjectManager : IGameObjectManager
    {
        /// <summary>
        /// 游戏对象对象池
        /// </summary>
        IGameObjectPool goPool;

        GameObject pools;

        IAssetsLoader assetsLoader;


        public DefaultGameObjectManager(IGameObjectPool goPool, IAssetsLoader assetsLoader)
        {
            if (goPool == null)
                throw new Exception(this.GetType().ToString() + " Inject TiktokGameObjectPool failed!");

            this.goPool = goPool;
            this.assetsLoader = assetsLoader;
        }

        /// <summary>
        /// 初始化，预加载一些常用的预制体，创建一个根节点，所有的游戏对象都挂在这个节点下，方便管理
        /// </summary>
        /// <param name="prefabsList"></param>
        /// <returns></returns>
        public async UniTask Initialize(List<string> prefabsList)
        {
            pools = new GameObject("Pools");

            var tasks = new List<UniTask>();
            foreach (var prefab in prefabsList)
            {
                if (goPool.HasRegist(prefab))
                    continue;

                var taskLevels = goPool.Regist(prefab, pools.transform, 10, OnCreate, OnRelease, OnRent, OnReturn);
                tasks.Add(taskLevels);
            }        

            await UniTask.WhenAll(tasks);
        }

        private void OnReturn(GameObject go)
        {
            go.SetActive(false);
            go.transform.SetParent(pools.transform) ;
        }

        private void OnRent(GameObject go)
        {
            go.SetActive(true);
        }

        /// <summary>
        /// 新的实例创建了
        /// </summary>
        /// <param name="go"></param>
        private void OnCreate(GameObject go)
        {
        }

        /// <summary>
        /// 实例是被释放掉了
        /// </summary>
        /// <param name="object"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnRelease(GameObject go)
        {
            GameObject.Destroy(go);
        }

        /// <summary>
        /// 租一个
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject Rent(string name, Transform parent)
        {
            return goPool.Rent(name, parent);
        }

        /// <summary>
        /// 还回一个实例到对象池
        /// </summary>
        /// <param name="go"></param>
        public void Return(GameObject go)
        {
            goPool.Return(go);
        }

        /// <summary>
        /// 实例化一个游戏对象，异步的，等同于Resources.LoadAsync + GameObject.Instantiate
        /// </summary>
        /// <param name="location"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public UniTask<GameObject> InstantiateAsync(string location, Transform parent)
        {
            return assetsLoader.InstantiateAsync(location, parent).AsUniTask();
        }
    }
}
