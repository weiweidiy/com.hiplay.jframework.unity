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
    }
}
