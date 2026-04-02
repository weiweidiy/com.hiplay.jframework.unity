using Game.Common;
using JFramework;

namespace JFramework.Unity
{
    public sealed class SceneContext : ISceneContext, IViewContext
    {
        public SceneContext(IServiceRegistry services)
        {
            Services = services;
        }

        public IServiceRegistry Services { get; }

        public IAssetsLoader Assets => Services.Resolve<IAssetsLoader>();

        public IJUIManager UI => Services.Resolve<IJUIManager>();

        public IGameObjectManager Objects => Services.Resolve<IGameObjectManager>();

        public IGameAudioManager Audio => Services.Resolve<IGameAudioManager>();

        public EventManager Events => Services.Resolve<EventManager>();

        public IControllerRegistry Controllers => Services.Resolve<IControllerRegistry>();

        public IModelRegistry Models => Services.Resolve<IModelRegistry>();

        public IViewRegistry Views => Services.Resolve<IViewRegistry>();
    }
}
