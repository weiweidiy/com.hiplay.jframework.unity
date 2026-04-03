using Cysharp.Threading.Tasks;
using System;

namespace JFramework.Unity
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseStateAsync : ISceneState
    {
        /// <summary>
        /// 上下文
        /// </summary>
        protected ISceneContext sceneContext;

        protected GameContext gameContext;

        /// <summary>
        /// 状态机名字
        /// </summary>
        public string Name => GetType().Name;


        public virtual UniTask EnterAsync(ISceneContext sceneContext, object arg)
        {

            this.sceneContext = sceneContext;
            gameContext = new GameContext
            {
                Services = sceneContext.Services
            };
            AddListeners();
            return OnEnter(arg);
        }

        public virtual UniTask ExitAsync()
        {
            RemoveListeners();
            return OnExit();
        }

        /// <summary>
        /// 子类实现
        /// </summary>
        /// <returns></returns>
        protected virtual UniTask OnEnter(object arg) => UniTask.CompletedTask;

        /// <summary>
        /// 状态退出时调用
        /// </summary>
        /// <returns></returns>
        public virtual UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }



        /// <summary>
        /// 事件监听器，在状态进入时调用，子类重写
        /// </summary>
        protected virtual void AddListeners() { }
        protected virtual void RemoveListeners() { }

        protected virtual IAssetsLoader GetAssetsLoader()
        {
            if (sceneContext?.Services != null &&
                sceneContext.Services.TryResolve<IAssetsLoader>(out var assetsLoader))
            {
                return assetsLoader;
            }

            throw new InvalidOperationException("IAssetsLoader is not registered in IServiceRegistry.");
        }

        protected virtual IJUIManager GetUIManager()
        {
            if (sceneContext?.Services != null &&
                sceneContext.Services.TryResolve<IJUIManager>(out var uiManager))
            {
                return uiManager;
            }

            throw new InvalidOperationException("IJUIManager is not registered in IServiceRegistry.");
        }
    }
}
