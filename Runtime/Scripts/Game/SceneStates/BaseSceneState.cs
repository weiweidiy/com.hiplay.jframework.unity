
using Cysharp.Threading.Tasks;
using Game.Common;
using JFramework;
using JFramework.Unity;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JFramework.Unity
{
    /// <summary>
    /// 默认的场景状态基类，提供了切换场景、初始化UI管理器、播放BGM等功能，子类只需要实现具体的场景类型、UI设置、BGM等信息即可
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseSceneState<TContext,TSceneType> : BaseStateAsync<TContext>
    {
        //[Inject]
        protected IAssetsLoader assetsLoader;
        //[Inject]
        //protected IInjectionContainer container;
        ///// <summary>
        ///// 游戏场景对象管理器
        ///// </summary>
        //[Inject]
        //protected TiktokGameObjectManager gameObjectManager;
        //[Inject]
        protected IJUIManager uiManager;
        //[Inject]
        //protected IGameAudioManager gameAudioManager;
        //[Inject]
        //protected TiktokConfigManager tiktokConfigManager;
        //[Inject]
        protected EventManager eventManager;
        //[Inject]
        protected List<ViewController> viewControllers;
        /// <summary>
        /// 状态参数
        /// </summary>
        protected object arg;

        
        public BaseSceneState(IAssetsLoader assetsLoader, IJUIManager uiManager, EventManager eventManager)
        {
            this.assetsLoader = assetsLoader;
            this.uiManager = uiManager;
            this.eventManager = eventManager;
        }

        protected override async UniTask OnEnter(object arg)
        {
            this.arg = arg;
            Debug.Log("Enter " + this.GetType());
            //切换到当前状态的场景
            var scene = await SwitchScene(GetSceneType().ToString(), SceneMode.Additive);

            //设置为活动场景
            SceneManager.SetActiveScene(scene);

            // 创建一个根节点，所有的游戏对象都挂在这个节点下，方便管理
            var root = new GameObject("GoRoot");
            //gameObjectManager.GoRoot = root;

            //初始化ui管理器
            await InitUiManager();

            // 启动所有的ViewController
            StartAllVeiwControllers();

            //播放场景BGM
            await PlayBGM();
        }

        public override async UniTask OnExit()
        {
            Debug.Log("OnExit " + this.GetType());
            //await base.OnExit();
            // 清理ViewController
            //viewControllers.Clear();

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
        protected virtual void StartAllVeiwControllers()
        {
            // 启动所有的ViewController
            foreach (var controller in GetControllers())
            {
                viewControllers.Add(controller);
            }

            OnInitializeVeiwControllers(viewControllers);

            foreach(var controller in viewControllers)
            {
                controller.OnStart();
            }

            //Debug.Log(GetSceneType().ToString() + " StartAllVeiwControllers done : " + viewControllers.Count());
        }

        /// <summary>
        /// 提供给子类重写，添加额外的ViewController
        /// </summary>
        /// <param name="viewControllers"></param>
        protected virtual void OnInitializeVeiwControllers(List<ViewController> viewControllers) { }

        /// <summary>
        /// 初始化UI管理器
        /// </summary>
        /// <returns></returns>
        protected virtual Task InitUiManager()
        {
            return uiManager.Initialize(GetUISettingsName());
        }

        /// <summary>
        /// 播放BGM
        /// </summary>
        /// <returns></returns>
        protected async UniTask PlayBGM()
        {
            //显示背景音乐
            //await gameAudioManager.PlayMusic(GetBGMClipName(), true, 0.5f);
        }

        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="sceneMode"></param>
        /// <returns></returns>
        protected UniTask<Scene> SwitchScene(string sceneName, SceneMode sceneMode)
        {
            return assetsLoader.LoadSceneAsync(sceneName, sceneMode).AsUniTask();
        }


        protected abstract ViewController[] GetControllers();

        protected abstract TSceneType GetSceneType();

        protected abstract string GetBGMClipName();

        protected abstract string GetUISettingsName();

        //void CheckInject()
        //{
        //    if (gameObjectManager == null)
        //        throw new System.Exception(this.GetType().ToString() + " Inject TiktokGameObjectManager failed!");

        //    if (uiManager == null)
        //        throw new System.Exception(this.GetType().ToString() + " Inject UIManager failed!");
        //}
    }
}
