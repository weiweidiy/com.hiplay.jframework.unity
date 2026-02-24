namespace JFramework.Unity
{
    public interface IViewControllerManager
    {
        void RegisterViewControllers();

        View[] GetViewControllers(string group);

        View GetViewController(string name);
    }
}
