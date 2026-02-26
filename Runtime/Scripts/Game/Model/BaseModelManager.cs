using JFramework.Unity;
using System.Collections.Generic;

namespace JFramework.Unity
{
    public abstract class BaseModelManager : IModelManager
    {
        protected Dictionary<string, object> models = new();
        public abstract void RegisterModels();
        public T GetModel<T>(string key) where T : class
        {
            if (models.TryGetValue(key, out var model))
            {
                return model as T;
            }
            return null;
        }
    }

}