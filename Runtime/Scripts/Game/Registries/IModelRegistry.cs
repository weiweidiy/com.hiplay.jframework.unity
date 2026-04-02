namespace JFramework.Unity
{
    public interface IModelRegistry
    {
        void Register<TModel>(TModel model) where TModel : class;

        TModel Get<TModel>() where TModel : class;

        bool TryGet<TModel>(out TModel model) where TModel : class;
    }
}
