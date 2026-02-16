namespace JFramework.Unity
{
    public interface IJFacade
    {
        IAssetsLoader GetAssetsLoader();

        IJUIManager GetUIManager();

        EventManager GetEventManager();

        IViewControllerManager GetViewControllerContainer();
    }
}
