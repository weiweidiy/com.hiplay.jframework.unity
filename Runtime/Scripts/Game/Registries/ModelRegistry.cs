using System;
using System.Collections.Generic;

namespace JFramework.Unity
{
    public sealed class ModelRegistry : IModelRegistry
    {
        private readonly Dictionary<Type, object> models = new();

        public void Register<TModel>(TModel model) where TModel : class
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            models[typeof(TModel)] = model;
        }

        public TModel Get<TModel>() where TModel : class
        {
            if (!TryGet<TModel>(out var model))
                throw new InvalidOperationException($"Model not found: {typeof(TModel).FullName}");

            return model;
        }

        public bool TryGet<TModel>(out TModel model) where TModel : class
        {
            if (models.TryGetValue(typeof(TModel), out var raw))
            {
                model = raw as TModel;
                return model != null;
            }

            model = null;
            return false;
        }
    }
}
