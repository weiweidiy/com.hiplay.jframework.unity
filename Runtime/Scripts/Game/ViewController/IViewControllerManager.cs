namespace JFramework.Unity
{
    public interface IViewControllerManager
    {
        void RegisterViewControllers();

        ViewController[] GetViewControllers(string group);

        ViewController GetViewController(string name);
    }
}
