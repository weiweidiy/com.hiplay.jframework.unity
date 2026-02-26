namespace JFramework.Unity
{
    public interface IViewManager
    {
        void RegisterViewControllers();

        View[] GetViewControllers(string group);

        View GetViewController(string name);
    }
}
