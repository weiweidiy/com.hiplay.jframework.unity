using Game.Common;
using JFramework;
using JFramework.Game;
using System;

namespace JFramework.Unity
{
    /// <summary>
    /// 用于安装默认的基础设施模块，提供一些常用的服务实现，如资源加载、事件总线、数据转换、HTTP请求、游戏对象池、配置管理、UI管理等。
    /// </summary>
    public class DefaultFoundationModule : IModuleInstaller
    {
        protected virtual IAssetsLoader InstallAssetsLoader(IServiceRegistry services)
        {
            if (!services.TryResolve<IAssetsLoader>(out var assetsLoader))
            {
                assetsLoader = new UnityResourcesAssetsLoader();
                services.AddSingleton<IAssetsLoader>(assetsLoader);
            }
            return assetsLoader;
        }

        protected virtual IJUIManager InstallUIManager(IServiceRegistry services, IAssetsLoader assetsLoader)
        {
            if (!services.TryResolve<IJUIManager>(out var uiManager))
            {
                uiManager = new DefaultUIManager(assetsLoader);
                services.AddSingleton<IJUIManager>(uiManager);
            }
            return uiManager;
        }

        protected virtual EventManager InstallEventManager(IServiceRegistry services)
        {
            if (!services.TryResolve<EventManager>(out var eventManager))
            {
                eventManager = new EventManager();
                services.AddSingleton(eventManager);
            }
            return eventManager;
        }

        protected virtual IEventBus InstallEventBus(IServiceRegistry services, EventManager eventManager)
        {
            if (!services.TryResolve<IEventBus>(out var eventBus))
            {
                eventBus = new EventBusAdapter(eventManager);
                services.AddSingleton<IEventBus>(eventBus);
            }
            return eventBus;
        }

        protected virtual IDataConverter InstallDataConverter(IServiceRegistry services)
        {
            if (!services.TryResolve<IDataConverter>(out var dataConverter))
            {
                dataConverter = new JDataConverter();
                services.AddSingleton<IDataConverter>(dataConverter);
            }
            return dataConverter;
        }

        protected virtual IHttpRequest InstallHttpRequest(IServiceRegistry services, IDataConverter dataConverter)
        {
            if (!services.TryResolve<IHttpRequest>(out var httpRequest))
            {
                httpRequest = new UnityWebRequestHttpRequest(dataConverter, new CustomCertificateHandler());
                services.AddSingleton<IHttpRequest>(httpRequest);
            }
            return httpRequest;
        }

        protected virtual IGameObjectPool InstallGameObjectPool(IServiceRegistry services, IAssetsLoader assetsLoader)
        {
            if (!services.TryResolve<IGameObjectPool>(out var gameObjectPool))
            {
                gameObjectPool = new XPoolGameObjectPool(assetsLoader);
                services.AddSingleton<IGameObjectPool>(gameObjectPool);
            }
            return gameObjectPool;
        }

        protected virtual IGameObjectManager InstallGameObjectManager(IServiceRegistry services, IGameObjectPool gameObjectPool, IAssetsLoader assetsLoader)
        {
            if (!services.TryResolve<IGameObjectManager>(out var gameObjectManager))
            {
                gameObjectManager = new DefaultGameObjectManager(gameObjectPool, assetsLoader);
                services.AddSingleton<IGameObjectManager>(gameObjectManager);
            }
            return gameObjectManager;
        }

        protected virtual IConfigLoader InstallConfigLoader(IServiceRegistry services)
        {
            if (!services.TryResolve<IConfigLoader>(out var configLoader))
            {
                configLoader = new UnityResourcesConfigLoader();
                services.AddSingleton<IConfigLoader>(configLoader);
            }
            return configLoader;
        }

        protected virtual IJConfigManager InstallJConfigManager(IServiceRegistry services, IConfigLoader configLoader, IDataConverter dataConverter)
        {
            if (!services.TryResolve<IJConfigManager>(out var jConfigManager))
            {
                jConfigManager = new JConfigManager(configLoader);
                services.AddSingleton<IJConfigManager>(jConfigManager);
            }
            return jConfigManager;
        }

        protected virtual ISpriteManager InstallSpriteManager(IServiceRegistry services, IAssetsLoader assetsLoader)
        {
            if (!services.TryResolve<ISpriteManager>(out var spriteManager))
            {
                spriteManager = new DefaultSpriteManager(assetsLoader);
                services.AddSingleton<ISpriteManager>(spriteManager);
            }
            return spriteManager;
        }

        protected virtual ITransitionProvider InstallTransitionProvider(IServiceRegistry services, IAssetsLoader assetsLoader)
        {
            if (!services.TryResolve<ITransitionProvider>(out var transitionProvider))
            {
                transitionProvider = new SMTransitionProvider(assetsLoader);
                services.AddSingleton<ITransitionProvider>(transitionProvider);
            }
            return transitionProvider;
        }

        protected virtual IGameAudioManager InstallGameAudioManager(IServiceRegistry services, IAssetsLoader assetsLoader)
        {
            if (!services.TryResolve<IGameAudioManager>(out var audioManager))
            {
                audioManager = new DefaultAudioManager(assetsLoader);
                services.AddSingleton<IGameAudioManager>(audioManager);
            }
            return audioManager;
        }

        protected virtual IDataManager InstallDataManager(IServiceRegistry services, IDataConverter dataConverter)
        {
            if (!services.TryResolve<IDataManager>(out var dataManager))
            {
                dataManager = new UnityPlayerPrefsDataManager(dataConverter);
                services.AddSingleton<IDataManager>(dataManager);
            }
            return dataManager;
        }

        protected virtual IGameDataStore InstallGameDataStore(IServiceRegistry services, IDataManager dataManager)
        {
            if (!services.TryResolve<IGameDataStore>(out var dataStore))
            {
                dataStore = new JDataStore(dataManager);
                services.AddSingleton<IGameDataStore>(dataStore);
            }
            return dataStore;
        }

        protected virtual ILanguageManager InstallLanguageManager(IServiceRegistry services)
        {
            if (!services.TryResolve<ILanguageManager>(out var languageManager))
            {
                Func<ILanguage,string> func = (l)=>l.Uid;
                languageManager = new JLanguageManager(func);
                services.AddSingleton<ILanguageManager>(languageManager);
            }
            return languageManager;
        }

        public virtual void Install(IServiceRegistry services)
        {
            var assetsLoader = InstallAssetsLoader(services);
            var uiManager = InstallUIManager(services, assetsLoader);
            var eventManager = InstallEventManager(services);
            var eventBus = InstallEventBus(services, eventManager);
            var dataConverter = InstallDataConverter(services);
            var httpRequest = InstallHttpRequest(services, dataConverter);
            var gameObjectPool = InstallGameObjectPool(services, assetsLoader);
            var gameObjectManager = InstallGameObjectManager(services, gameObjectPool, assetsLoader);
            var configLoader = InstallConfigLoader(services);
            var jConfigManager = InstallJConfigManager(services, configLoader, dataConverter);
            var spriteManager = InstallSpriteManager(services, assetsLoader);
            var transitionProvider = InstallTransitionProvider(services, assetsLoader);
            var audioManager = InstallGameAudioManager(services, assetsLoader);
            var dataManager = InstallDataManager(services, dataConverter);
            var dataStore = InstallGameDataStore(services, dataManager);
            var languageManager = InstallLanguageManager(services);
        }
    }
}