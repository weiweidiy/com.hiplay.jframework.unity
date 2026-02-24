namespace JFramework.Unity
{
    public interface IControllerManager
    {
        void RegisterControllers();
        Controller GetController(string name);
    }
}
