using Cysharp.Threading.Tasks;
using JFramework.Game;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace JFramework.Unity
{
    public class JFacade : IJUIManager, IJNetworkable, ISceneStateMachineAsync, IJFacade, IGameObjectManager, ISpriteManager
    {
        /// <summary>
        /// UI管理器
        /// </summary>
        IJUIManager uiManager;

        /// <summary>
        /// 网络管理器
        /// </summary>
        IJNetwork networkManager;

        /// <summary>
        /// 资源加载器
        /// </summary>
        IAssetsLoader assetsLoader;

        /// <summary>
        /// 消息事件管理器
        /// </summary>
        EventManager eventManager;

        /// <summary>
        /// 场景状态机
        /// </summary>
        ISceneStateMachineAsync sm;

        /// <summary>
        /// 第一个场景状态，框架启动后会自动切换到这个状态，通常是登录场景
        /// </summary>
        string firstSceneState;

        /// <summary>
        /// 游戏上下文
        /// </summary>
        GameContext context;

        /// <summary>
        /// 视图控制器容器
        /// </summary>
        IViewManager viewControllerManager;

        /// <summary>
        /// 模型容器，负责管理游戏中的数据模型，提供数据访问和更新的接口，和场景状态机解耦，允许在不同的场景状态中复用同一个模型容器
        /// </summary>
        IModelManager modelManager;

        /// <summary>
        /// 控制器管理器
        /// </summary>
        IControllerManager controllerManager;

        /// <summary>
        /// 游戏对象管理器
        /// </summary>
        IGameObjectManager gameObjectManager;

        /// <summary>
        /// 精灵管理器
        /// </summary>
        ISpriteManager spriteManager;

        /// <summary>
        /// http请求接口，提供发送http请求的功能，允许在游戏中方便地进行网络通信，获取服务器数据等操作
        /// </summary>
        IHttpRequest httpRequest;

        /// <summary>
        /// 配置表管理器
        /// </summary>
        IJConfigManager configManager;

        /// <summary>
        /// 游戏资源快捷查询接口，提供快速查询游戏资源的功能，允许在游戏中方便地获取各种资源，如预制体、音频、特效等，提升资源管理的效率和便利性
        /// </summary>
        IGameAssetsQuary gameAssetsQuary;

        /// <summary>
        /// 转场提供者，提供转场动画的功能，允许在场景切换时显示转场动画，提升游戏的视觉效果和用户体验
        /// </summary>
        ITransitionProvider transitionProvider;

        public JFacade(IJUIManager uiManager, IJNetwork networkManager, IAssetsLoader assetsLoader, EventManager eventManager
            , ISceneStateMachineAsync sm, string firstSceneState, GameContext context, IGameObjectManager gameObjectManager
            , IModelManager modelManager, IViewManager viewControllerContainer, IControllerManager controllerManager
            , IHttpRequest httpRequest, IJConfigManager configManager, ISpriteManager spriteManager, IGameAssetsQuary gameAssetsQuary
            ,ITransitionProvider transitionProvider)
        {
            this.networkManager = networkManager;
            this.uiManager = uiManager;
            this.assetsLoader = assetsLoader;
            this.eventManager = eventManager;
            this.sm = sm;
            this.firstSceneState = firstSceneState;
            this.context = context;
            this.context.Facade = this;
            this.gameObjectManager = gameObjectManager;
            this.viewControllerManager = viewControllerContainer;
            this.modelManager = modelManager;
            this.controllerManager = controllerManager;
            this.httpRequest = httpRequest;
            this.configManager = configManager;
            this.spriteManager = spriteManager;
            this.gameAssetsQuary = gameAssetsQuary;
            if(this.gameAssetsQuary != null)
                this.gameAssetsQuary.SetFacade(this);
            this.transitionProvider = transitionProvider;
        }

        /// <summary>
        /// 运行游戏
        /// </summary>
        /// <param name="beforeSwitchState">可以在切换状态前进行预加载，参数是IJConfigManager</param>
        /// <returns></returns>
        public async Task Run(Func<IJConfigManager, UniTask> beforeSwitchState = null)
        {
            UniTask taskLoadConfigs = UniTask.CompletedTask;
            if (configManager != null)
                taskLoadConfigs = this.configManager.PreloadAllAsync().AsUniTask();

            await taskLoadConfigs;

            this.modelManager.RegisterModels();
            this.viewControllerManager.RegisterViewControllers();
            this.controllerManager.RegisterControllers();

            if (beforeSwitchState != null)
                await beforeSwitchState(configManager);
            else
                await UniTask.CompletedTask;

            await SwitchToState(firstSceneState, context);
        }

        #region Facade接口
        public IAssetsLoader GetAssetsLoader() => assetsLoader;

        public IJUIManager GetUIManager() => uiManager;

        public EventManager GetEventManager() => eventManager;

        public IViewManager GetViewControllerContainer() => viewControllerManager;

        public IModelManager GetModelManager()=> modelManager;

        public IControllerManager GetControllerManager()=> controllerManager;

        public ISceneStateMachineAsync GetSceneStateMachine() => sm;

        public IHttpRequest GetHttpRequest()=> httpRequest;

        public IGameObjectManager GetGameObjectManager() => gameObjectManager;
        public ISpriteManager GetSpriteManager()=> spriteManager;
        public IJConfigManager GetConfigManager() => configManager;

        public IGameAssetsQuary GetGameAssetsQuary() => gameAssetsQuary;

        public ITransitionProvider GetTransitionProvider() => transitionProvider;

        public IJNetwork GetNetworkManager() => networkManager;

        public async UniTask<ITransition> TransitonOut(string transitionType)
        {
            var transition = await transitionProvider.InstantiateAsync(transitionType);
            await transition.TransitionOut();
            return transition;
        }
        

        public async UniTask TransitonIn(ITransition transition) => await transition.TransitionIn();

        #endregion

        #region 场景状态机接口
        public UniTask SwitchToState(string stateName, GameContext context)
        {
            return sm.SwitchToState(stateName, context);
        }
        #endregion

        #region 游戏对象管理器接口
        public UniTask PreloadGameObjects(List<string> prefabsList)
        {
            return gameObjectManager.PreloadGameObjects(prefabsList);
        }

        public GameObject Rent(string name, Transform parent)
        {
            return gameObjectManager.Rent(name, parent);
        }

        public void Return(GameObject go)
        {
            gameObjectManager.Return(go);
        }

        public UniTask<GameObject> InstantiateAsync(string location, Transform parent)
        {
            return gameObjectManager.InstantiateAsync(location, parent);
        }
        #endregion

        #region 精灵管理器接口
        public UniTask PreloadSprites(List<string> spritesList)
        {
            return spriteManager.PreloadSprites(spritesList);
        }

        public Sprite GetSprite(string name)
        {
            return spriteManager.GetSprite(name);
        }

        public UniTask<Sprite> LoadSpriteAsync(string location)
        {
            return spriteManager.LoadSpriteAsync(location);
        }
        #endregion

        #region UI接口
        public Task Initialize(string uiSettingName)
        {
            return uiManager.Initialize(uiSettingName);
        }

        public void CloseWindow(string screenId)
        {
            uiManager.CloseWindow(screenId);
        }

        public Camera GetUICamera()
        {
            return uiManager.GetUICamera();
        }

        public Canvas GetUIFrameCanvas()
        {
            return uiManager.GetUIFrameCanvas();
        }

        public void HidePanel(string screenId)
        {
            uiManager.HidePanel(screenId);
        }


        public bool IsPanelOpen(string screenId)
        {
            return uiManager.IsPanelOpen(screenId);
        }

        public IWindowController OpenWindow(string screenId)
        {
            return uiManager.OpenWindow(screenId);
        }

        public IWindowController OpenWindow<TArg>(string screenId, TArg properties) where TArg : IWindowProperties
        {
            return uiManager.OpenWindow(screenId, properties);
        }

        public IPanelController ShowPanel(string screenId, bool asNew = false)
        {
            return uiManager.ShowPanel(screenId, asNew);
        }

        public IPanelController ShowPanel<TArg>(string screenId, TArg properties, bool asNew = false) where TArg : IPanelProperties
        {
            return uiManager.ShowPanel(screenId, properties, asNew);
        }


        #endregion

        #region 网络接口
        public bool IsConnecting()
        {
            if (networkManager == null)
            {
                Debug.LogError("[JFacade] Network manager is not set!");
                return false;
            }
            return networkManager.IsConnecting();
        }

        Task<TResponse> IJNetworkable.SendMessage<TResponse>(IJNetMessage pMsg, TimeSpan? timeout)
        {
            if (networkManager == null)
            {
                Debug.LogError("[JFacade] Network manager is not set!");
                return Task.FromResult(default(TResponse));
            }
            return networkManager.SendMessage<TResponse>(pMsg, timeout);
        }

        public Task Connect(string url, string token = null)
        {
            if (networkManager == null)
            {
                Debug.LogError("[JFacade] Network manager is not set!");
                return Task.CompletedTask;
            }
            return networkManager.Connect(url, token);
        }

        public void Disconnect()
        {
            if (networkManager != null)
            {
                Debug.LogWarning("[JFacade] Network manager is not set!");
                return;
            }
            networkManager.Disconnect();
        }

  











        #endregion
    }
}
