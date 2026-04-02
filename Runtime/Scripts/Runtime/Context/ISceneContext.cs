using Game.Common;
using JFramework;

namespace JFramework.Unity
{
    public interface ISceneContext
    {
        IAssetsLoader Assets { get; }

        IJUIManager UI { get; }

        IGameObjectManager Objects { get; }

        IGameAudioManager Audio { get; }

        EventManager Events { get; }

        IControllerRegistry Controllers { get; }

        IModelRegistry Models { get; }

        IViewRegistry Views { get; }

        IServiceRegistry Services { get; }
    }
}
