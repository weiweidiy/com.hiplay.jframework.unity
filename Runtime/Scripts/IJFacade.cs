using Cysharp.Threading.Tasks;
using JFramework.Game;

namespace JFramework.Unity
{
    public interface IJFacade
    {
        IAssetsLoader GetAssetsLoader();

        IJUIManager GetUIManager();

        EventManager GetEventManager();

        IViewManager GetViewControllerContainer();

        IModelManager GetModelManager();

        IControllerManager GetControllerManager();

        ISceneStateMachineAsync GetSceneStateMachine();

        IHttpRequest GetHttpRequest();

        IGameObjectManager GetGameObjectManager();

        ISpriteManager GetSpriteManager();

        IJConfigManager GetConfigManager();

        IGameAssetsQuary GetGameAssetsQuary();

        ITransitionProvider GetTransitionProvider();

        UniTask<ITransition> TransitonOut(string transitionType);

        UniTask TransitonIn(ITransition transition);

        IJNetwork GetNetworkManager();
    }
}
