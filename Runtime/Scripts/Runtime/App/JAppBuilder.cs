using System;
using System.Collections.Generic;
using System.Linq;

namespace JFramework.Unity
{
    /// <summary>
    /// app构建器，负责收集模块安装器和服务实例，并构建JApp实例
    /// </summary>
    public sealed class JAppBuilder
    {
        private readonly List<IModuleInstaller> modules = new();
        private readonly ServiceRegistry services = new();
        private Type firstSceneStateType;

        /// <summary>
        /// 添加模块安装器，模块安装器负责向服务注册表中注册模块所需的服务
        /// </summary>
        /// <param name="moduleInstaller"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public JAppBuilder AddModule(IModuleInstaller moduleInstaller)
        {
            if (moduleInstaller == null)
                throw new ArgumentNullException(nameof(moduleInstaller));

            modules.Add(moduleInstaller);
            return this;
        }

        /// <summary>
        /// 添加服务实例，服务实例将被注册为单例，并可通过 IJApp.GetService<T> 方法获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <returns></returns>
        public JAppBuilder AddService<T>(T service) where T : class
        {
            services.AddSingleton(service);
            return this;
        }

        /// <summary>
        /// 设置第一个场景状态类型，如果设置了第一个场景状态类型，JApp.RunAsync 方法将自动进入该场景状态
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <returns></returns>
        public JAppBuilder SetFirstState<TState>() where TState : class, ISceneState
        {
            firstSceneStateType = typeof(TState);
            return this;
        }

        public IJApp Build()
        {
            foreach (var module in modules)
            {
                module.Install(services);
            }

            BindAwareServices();

            //设置默认的场景上下文，如果用户没有注册的话，确保场景流程能够正常工作
            if (!services.TryResolve<ISceneContext>(out _))
            {
                var sceneContext = new SceneContext(services);
                services.AddSingleton<ISceneContext>(sceneContext);
                services.AddSingleton<IViewContext>(sceneContext);
            }

            if (firstSceneStateType != null && services.TryResolve<ISceneStateRegistry>(out var stateRegistry))
            {
                if (!services.TryResolve<ISceneFlow>(out _))
                {
                    services.AddSingleton<ISceneFlow>(new SceneFlowService(stateRegistry, services.Resolve<ISceneContext>(), firstSceneStateType));
                }
            }

            var allServices = services.GetAllServices();
            // 从服务注册表中获取所有实现了 IInitializable 和 IAsyncDisposableModule 接口的服务实例，分别存储在 initializables 和 disposables 列表中
            var initializables = allServices.OfType<IInitializable>().ToList();
            var disposables = allServices.OfType<IAsyncDisposableModule>().ToList();
            return new JApp(services, initializables, disposables);
        }

        /// <summary>
        /// 绑定服务实例到实现了 IServiceRegistryAware 接口的服务上，使它们能够访问服务注册表
        /// </summary>
        private void BindAwareServices()
        {
            foreach (var service in services.GetAllServices())
            {
                if (service is IServiceRegistryAware aware)
                {
                    aware.Services = services;
                }
            }
        }
    }
}
