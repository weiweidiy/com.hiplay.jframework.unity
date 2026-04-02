
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JFramework.Unity
{
    /// <summary>
    /// 默认的场景状态基类，提供了切换场景、初始化UI管理器、播放BGM等功能，子类只需要实现具体的场景类型、UI设置、BGM等信息即可
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseSceneState<TSceneType> : BaseStateAsync
    {
 
        protected List<View> viewControllers = new List<View>();
        /// <summary>
        /// 状态参数
        /// </summary>
        protected object arg;

        /// <summary>
        /// 游戏对象根节点，所有的游戏对象都挂在这个节点下，方便管理和清理
        /// </summary>
        protected Transform goRoot;

        protected override async UniTask OnEnter(object arg)
        {
            this.arg = arg;
            Debug.Log("Enter " + this.GetType());
            try
            {
                //切换到当前状态的场景
                var scene = await SwitchScene(GetSceneType().ToString(), SceneMode.Additive);
                //设置为活动场景
                SceneManager.SetActiveScene(scene);
            }
            catch (System.Exception e)
            {
                Debug.LogError("切换场景失败 " + GetSceneType().ToString() + " " + e.ToString());
                throw;
            }


            // 创建一个根节点，所有的游戏对象都挂在这个节点下，方便管理
            goRoot = (new GameObject("GoRoot")).transform;
            //gameObjectManager.GoRoot = root;

            //初始化ui管理器
            await GetUIManager().Initialize(GetUISettingsName());

            // 启动所有的ViewController
            StartAllVeiwControllers();

            //播放场景BGM
            await PlayBGM();
        }

        public override async UniTask OnExit()
        {
            Debug.Log("OnExit " + this.GetType());

            // 清理ViewController
            StopAllViewControllers();

            // 卸载当前场景
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(GetSceneType().ToString());

            // 可选：等待卸载完成
            while (!asyncUnload.isDone)
            {
                await UniTask.Delay(100);
            }
            await base.OnExit();
        }

        /// <summary>
        /// 初始化场景所有的ViewController
        /// </summary>
        protected  void StartAllVeiwControllers()
        {
            // 启动所有的ViewController
            var views = GetViews();
            if(views == null || views.Length == 0)
            {
                Debug.LogWarning("当前场景没有ViewController " + GetSceneType().ToString());
                return;
            }

            foreach (var view in views)
            {
                viewControllers.Add(view);
                view.Start(gameContext);
            }
        }

        protected void StopAllViewControllers()
        {
            foreach (var controller in viewControllers)
            {
                controller.Stop();
            }
            viewControllers.Clear();
        }


        /// <summary>
        /// 播放BGM
        /// </summary>
        /// <returns></returns>
        protected async UniTask PlayBGM()
        {
            //显示背景音乐
            //await gameAudioManager.PlayMusic(GetBGMClipName(), true, 0.5f);
            await UniTask.CompletedTask;
        }

        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="sceneMode"></param>
        /// <returns></returns>
        protected virtual UniTask<Scene> SwitchScene(string sceneName, SceneMode sceneMode)
        {
            return GetAssetsLoader().LoadSceneAsync(sceneName, sceneMode).AsUniTask();
        }

        /// <summary>
        /// 获取当前状态下所有注册的ViewController，子类可以通过GetController<TView>()方法获取指定类型的ViewController
        /// </summary>
        /// <returns></returns>
        protected View[] GetViews()
        {
            if (sceneContext?.Services != null &&
                sceneContext.Services.TryResolve<IViewRegistry>(out var viewRegistry))
            {
                var views = viewRegistry.GetViewsForScene(GetType());
                if (views != null && views.Count > 0)
                    return views.ToArray();
            }

            return Array.Empty<View>();
        }

        /// <summary>
        /// 获取指定类型的ViewController，如果当前场景没有该类型的ViewController，则返回null
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <returns></returns>
        protected TView GetController<TView>() where TView : View
        {
            try
            {
                return viewControllers.Where((ctrl) => ctrl is TView).FirstOrDefault() as TView;
            }
            catch(Exception e)
            {
                Debug.LogError("GetController " + typeof(TView).ToString() + " failed! " + e.ToString());
                throw;
            }
        }

        protected abstract TSceneType GetSceneType();

        protected abstract string GetBGMClipName();

        protected abstract string GetUISettingsName();

        //void CheckInject()
        //{
        //    if (gameObjectManager == null)
        //        throw new System.Exception(this.GetType().ToString() + " Inject TiktokGameObjectManager failed!");

        //    if (uiManager == null)
        //        throw new System.Exception(this.GetType().ToString() + " Inject uiManager failed!");
        //}
    }
}
