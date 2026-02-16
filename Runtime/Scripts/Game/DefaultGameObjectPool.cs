using Cysharp.Threading.Tasks;
using MackySoft.XPool.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework.Unity
{
    /// <summary>
    /// GameObject对象池
    /// </summary>
    public class DefaultGameObjectPool : IGameObjectPool
    {
        /// <summary>
        /// 缓存对象池
        /// </summary>
        Dictionary<string, GameObjectPool> originObject = new Dictionary<string, GameObjectPool>();

        /// <summary>
        /// 资源加载器,提供加载场景和预制体的功能,具体实现可以使用Addressable或者Resources等方式
        /// </summary>
        IAssetsLoader assetsLoader;

        public DefaultGameObjectPool(IAssetsLoader assetsLoader)
        {
            this.assetsLoader = assetsLoader;
        }

        /// <summary>
        /// 注册一个对象池
        /// </summary>
        /// <param name="location"></param>
        /// <param name="capacity"></param>
        /// <param name="onCreate">rent时候如果新创建实例则调用</param>
        /// <param name="onRelease"></param>
        /// <param name="onRent"></param>
        /// <param name="onReturn"></param>
        /// <exception cref="System.Exception"></exception>
        public async UniTask Regist(string location , Transform root = null, int capacity = 10 
                                , Action<GameObject> onCreate = null
                                , Action<GameObject> onRelease = null
                                , Action<GameObject> onRent = null
                                , Action<GameObject> onReturn = null)
        {
            if (originObject.ContainsKey(location))
                throw new Exception("已经注册了对象 " + location);

            var go = await assetsLoader.InstantiateAsync(location, root);
            var goPool = new GameObjectPool(go, capacity);
            goPool.OnCreate = onCreate;
            goPool.OnRelease = onRelease;
            goPool.OnRent = onRent;
            goPool.OnReturn = onReturn;
            go.name = location;

            goPool.Return(go);
            originObject.Add(location, goPool);
            //隐藏起来
            go.SetActive(false);

            
        }

        public bool HasRegist(string location)
        {
            return originObject.ContainsKey(location);
        }

        /// <summary>
        /// 反注册
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool UnRegist(string location)
        {

            if (!originObject.ContainsKey(location))
                throw new Exception("没有找到需要反注册的对象 " + location);

            //释放掉
            var goPool = originObject[location];
            goPool.ReleaseInstances(goPool.Capacity);

            return originObject.Remove(location); //池子里的游戏对象没有移除
        }

        /// <summary>
        /// 反注册
        /// </summary>
        public void UnRegistAll()
        {
            foreach(var key in originObject.Keys)
            {
                UnRegist(key);
            }

            originObject.Clear();
        }

        /// <summary>
        /// 获取一个游戏对象
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public GameObject Rent(string location)
        {
            return Rent(location,null);
        }

        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <param name="location"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public GameObject Rent(string location, Transform parent)
        {

            if (!originObject.ContainsKey(location))
                throw new System.Exception("没有指定的对象池" + location);

            var go = originObject[location].Rent();
            go.name = location;
            go.transform.SetParent(parent);
            return go;
        }

        /// <summary>
        /// 返回到对象池(注意，游戏对象不能改名）
        /// </summary>
        /// <param name="obj"></param>
        public void Return(GameObject obj)
        {
            var key = obj.name;

            if (!originObject.ContainsKey(key))
                throw new System.Exception("没有指定的对象池" + key);

            originObject[key].Return(obj);
        }


    }
}

