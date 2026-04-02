using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace JFramework.Unity
{
    public sealed class JApp : IJApp
    {
        private readonly IServiceRegistry services;
        private readonly List<IInitializable> initializables;
        private readonly List<IAsyncDisposableModule> disposables;

        public JApp(
            IServiceRegistry services,
            List<IInitializable> initializables,
            List<IAsyncDisposableModule> disposables)
        {
            this.services = services;
            this.initializables = initializables;
            this.disposables = disposables;
        }

        /// <summary>
        /// 获取服务实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>() where T : class
        {
            return services.Resolve<T>();
        }

        /// <summary>
        /// 运行应用程序，依次调用所有模块的 InitializeAsync 方法，并进入第一个场景状态
        /// </summary>
        /// <returns></returns>
        public async UniTask RunAsync()
        {
            // 依次调用所有模块的 InitializeAsync 方法
            foreach (var initializable in initializables)
            {
                await initializable.InitializeAsync();
            }

            // 进入第一个场景状态
            if (services.TryResolve<ISceneFlow>(out var sceneFlow))
            {
                await sceneFlow.EnterFirstAsync();
            }
        }

        /// <summary>
        /// 关闭应用程序，依次调用所有模块的 DisposeAsync 方法
        /// </summary>
        /// <returns></returns>
        public async UniTask ShutdownAsync()
        {
            for (int i = disposables.Count - 1; i >= 0; i--)
            {
                await disposables[i].DisposeAsync();
            }
        }
    }
}
