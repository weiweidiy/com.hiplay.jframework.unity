namespace JFramework.Unity
{
    public interface IModelManager
    {
        void RegisterModels();

        T GetModel<T>(string key) where T : class;
    }
}
