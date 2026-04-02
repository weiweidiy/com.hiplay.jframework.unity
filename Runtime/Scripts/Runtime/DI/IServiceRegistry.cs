using System;
using System.Collections.Generic;

namespace JFramework.Unity
{
    public interface IServiceRegistry
    {
        void AddSingleton<T>(T instance) where T : class;

        void AddSingleton(Type serviceType, object instance);

        T Resolve<T>() where T : class;

        object Resolve(Type serviceType);

        bool TryResolve<T>(out T service) where T : class;

        bool TryResolve(Type serviceType, out object service);

        IReadOnlyCollection<object> GetAllServices();
    }
}
