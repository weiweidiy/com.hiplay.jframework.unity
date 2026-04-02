using System;
using System.Collections.Generic;

namespace JFramework.Unity
{
    /// <summary>
    /// 服务注册表，负责存储和管理服务实例，并提供服务解析功能
    /// </summary>
    public sealed class ServiceRegistry : IServiceRegistry
    {
        private readonly Dictionary<Type, object> services = new();

        /// <summary>
        /// 添加服务实例，服务实例将被注册为单例，并可通过 IJApp.GetService<T> 方法获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public void AddSingleton<T>(T instance) where T : class
        {
            AddSingleton(typeof(T), instance);
        }

        /// <summary>
        /// 添加服务实例，服务实例将被注册为单例，并可通过 IJApp.GetService<T> 方法获取
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void AddSingleton(Type serviceType, object instance)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (!serviceType.IsInstanceOfType(instance))
                throw new ArgumentException($"Instance is not assignable to {serviceType.Name}", nameof(instance));

            services[serviceType] = instance;
        }

        public T Resolve<T>() where T : class
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type serviceType)
        {
            if (!TryResolve(serviceType, out var service))
                throw new InvalidOperationException($"Service not found: {serviceType.FullName}");

            return service;
        }

        public bool TryResolve<T>(out T service) where T : class
        {
            if (TryResolve(typeof(T), out var raw))
            {
                service = raw as T;
                return service != null;
            }

            service = null;
            return false;
        }

        public bool TryResolve(Type serviceType, out object service)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            return services.TryGetValue(serviceType, out service);
        }

        public IReadOnlyCollection<object> GetAllServices()
        {
            return services.Values;
        }
    }
}
