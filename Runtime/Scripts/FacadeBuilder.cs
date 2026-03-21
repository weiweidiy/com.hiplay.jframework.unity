using Game.Common;
using JFramework.Game;
using UnityEngine;

namespace JFramework.Unity
{
    public class FacadeBuilder
    {
        IJUIManager uiManager;

        IJNetwork networkManager;

        IAssetsLoader assetsLoader;

        EventManager eventManager;

        ISceneStateMachineAsync sm;

        string firstSceneState;

        GameContext context;

        IViewManager viewControllerManager;

        BaseModelManager modelManager;

        IControllerManager controllerManager;

        IGameObjectPool gameObjectPool;

        IGameObjectManager gameObjectManager;

        IHttpRequest httpRequest;

        IDataConverter dataConverter;

        IJConfigManager configManager;

        IConfigLoader configLoader;

        ISpriteManager spriteManager;

        IGameAssetsQuary gameAssetsQuary;

        ITransitionProvider transitionProvider;

        IGameAudioManager gameAudioManager;

        BaseNetworkMessageHandler networkMessageHandler;

        JNetworkBuilder networkBuilder;

        public void UseNetworkSocket(bool useSocket)
        {
            networkBuilder = useSocket ? new JNetworkBuilder() : null;
        }

        public JFacade Build()
        {
            if (assetsLoader == null)
            {
                assetsLoader = new DefaultAssetsLoader();
            }

            if (uiManager == null)
            {
                uiManager = new DefaultUIManager(assetsLoader);
            }

            if (eventManager == null)
            {
                eventManager = new EventManager();
            }

            if (context == null)
            {
                context = new GameContext();
            }

            if (sm == null)
            {
                throw new System.Exception("SceneStateMachine is required but not set in FacadeBuilder!");
            }

            if (firstSceneState == null)
            {
                throw new System.Exception("FirstSceneState is required but not set in FacadeBuilder!");
            }

            if (viewControllerManager == null)
            {
                throw new System.Exception("ViewControllerContainer is required but not set in FacadeBuilder!");
            }

            if (modelManager == null)
            {
                throw new System.Exception("ModelManager is required but not set in FacadeBuilder!");
            }

            if (controllerManager == null)
            {
                throw new System.Exception("ControllerManager is required but not set in FacadeBuilder!");
            }

            if (gameAudioManager == null)
            {
                gameAudioManager = new DefaultAudioManager(assetsLoader);
            }

            if (dataConverter == null)
            {
                dataConverter = new JDataConverter();
            }

            if (httpRequest == null)
            {
                httpRequest = new DefaultHttpRequest(dataConverter, new CustomCertificateHandler());
            }

            if (gameObjectPool == null)
            {
                gameObjectPool = new DefaultGameObjectPool(assetsLoader);
            }

            if (gameObjectManager == null)
            {
                gameObjectManager = new DefaultGameObjectManager(gameObjectPool, assetsLoader);
            }

            if (configLoader == null)
            {
                configLoader = new DefaultConfigLoader();
            }

            if (configManager == null)
            {
                configManager = new JConfigManager(configLoader);
            }

            if (spriteManager == null)
            {
                spriteManager = new DefaultSpriteManager(assetsLoader);
            }

            if (gameAssetsQuary == null)
            {
                Debug.LogWarning("GameAssetsQuary is not set in FacadeBuilder, using DefaultGameAssetsQuary. You can set a custom GameAssetsQuary by calling SetGameAssetsQuary method in FacadeBuilder.");
                //gameAssetsQuary = new DefaultGameAssetsQuary(assetsLoader);
            }

            if (transitionProvider == null)
            {
                transitionProvider = new SMTransitionProvider(assetsLoader);
            }


            if (networkManager == null && networkBuilder != null)
            {
                networkBuilder.SetDataConverter(dataConverter);
                networkManager = networkBuilder.Build();
                //networkManager = new JNetwork(socketFactory, new JTaskCompletionSourceManager<IUnique>(), networkMessageProcessStrate, networkMessageHandler);
            }

            var facade = new JFacade(uiManager, networkManager, assetsLoader, eventManager, sm, firstSceneState, context
                    , gameObjectManager, modelManager, viewControllerManager, controllerManager, httpRequest, configManager, spriteManager, gameAssetsQuary
                    , transitionProvider, gameAudioManager);

            if (networkMessageHandler != null)
            {
                networkMessageHandler.Facade = facade;
            }

            if (modelManager != null)
            {
                modelManager.Facade = facade;
            }

            return facade;
        }

        public FacadeBuilder SetAssetsLoader(IAssetsLoader assetsLoader)
        {
            this.assetsLoader = assetsLoader;
            return this;
        }

        public FacadeBuilder SetUIManager(IJUIManager uiManager)
        {
            this.uiManager = uiManager;
            return this;
        }

        public FacadeBuilder SetNetworkManager(IJNetwork networkManager)
        {
            this.networkManager = networkManager;
            return this;
        }

        public FacadeBuilder SetEventManager(EventManager eventManager)
        {
            this.eventManager = eventManager;
            return this;
        }

        public FacadeBuilder SetSceneStateMachine(ISceneStateMachineAsync sm)
        {
            this.sm = sm;
            return this;
        }

        public FacadeBuilder SetFirstSceneState(string firstSceneState)
        {
            this.firstSceneState = firstSceneState;
            return this;
        }

        public FacadeBuilder SetGameContext(GameContext context)
        {
            this.context = context;
            return this;
        }

        public FacadeBuilder SetViewControllerContainer(IViewManager viewControllerContainer)
        {
            this.viewControllerManager = viewControllerContainer;
            return this;
        }

        public FacadeBuilder SetModelManager(BaseModelManager modelManager)
        {
            this.modelManager = modelManager;
            return this;
        }

        public FacadeBuilder SetControllerManager(IControllerManager controllerManager)
        {
            this.controllerManager = controllerManager;
            return this;
        }

        public FacadeBuilder SetGameObjectPool(IGameObjectPool gameObjectPool)
        {
            this.gameObjectPool = gameObjectPool;
            return this;
        }

        public FacadeBuilder SetGameObjectManager(IGameObjectManager gameObjectManager)
        {
            this.gameObjectManager = gameObjectManager;
            return this;
        }

        public FacadeBuilder SetHttpRequest(IHttpRequest httpRequest)
        {
            this.httpRequest = httpRequest;
            return this;
        }

        public FacadeBuilder SetDataConverter(IDataConverter dataConverter)
        {
            this.dataConverter = dataConverter;

            return this;
        }

        public FacadeBuilder SetConfigManager(IJConfigManager configManager)
        {
            this.configManager = configManager;
            return this;
        }

        public FacadeBuilder SetConfigLoader(IConfigLoader configLoader)
        {
            this.configLoader = configLoader;
            return this;
        }

        public FacadeBuilder SetSpriteManager(ISpriteManager spriteManager)
        {
            this.spriteManager = spriteManager;
            return this;
        }

        public FacadeBuilder SetGameAssetsQuary(IGameAssetsQuary gameAssetsQuary)
        {
            this.gameAssetsQuary = gameAssetsQuary;
            return this;
        }

        public FacadeBuilder SetTransitionProvider(ITransitionProvider transitionProvider)
        {
            this.transitionProvider = transitionProvider;
            return this;
        }

        public FacadeBuilder SetSocketFactory(ISocketFactory socketFactory)
        {
            if (networkBuilder == null)
            {
                UseNetworkSocket(true);
            }

            networkBuilder.SetSocketFactory(socketFactory);
            //this.socketFactory = socketFactory;
            return this;
        }

        public FacadeBuilder SetSocket(IJSocket socket)
        {
            if (networkBuilder == null)
            {
                UseNetworkSocket(true);
            }
            networkBuilder.SetSocket(socket);
            //this.socket = socket;
            return this;
        }

        public FacadeBuilder SetNetworkMessageProcessStrate(INetworkMessageProcessStrate networkMessageProcessStrate)
        {
            if (networkBuilder == null)
            {
                UseNetworkSocket(true);
            }

            networkBuilder.SetMessageProcessStrate(networkMessageProcessStrate);
            //this.networkMessageProcessStrate = networkMessageProcessStrate;
            return this;
        }

        public FacadeBuilder SetNetMessageSerializerStrate(INetMessageSerializerStrate netMessageSerializerStrate)
        {
            if (networkBuilder == null)
            {
                UseNetworkSocket(true);
            }
            networkBuilder.SetNetMessageSerializerStrate(netMessageSerializerStrate);
            //this.netMessageSerializerStrate = netMessageSerializerStrate;
            return this;
        }

        public FacadeBuilder SetMessageTypeResolver(IMessageTypeResolver messageTypeResolver)
        {
            if (networkBuilder == null)
            {
                UseNetworkSocket(true);
            }
            networkBuilder.SetMessageTypeResolver(messageTypeResolver);
            //this.messageTypeResolver = messageTypeResolver;
            return this;
        }

        public FacadeBuilder SetProtocolRegister(ITypeRegister protocolRegister)
        {
            if (networkBuilder == null)
            {
                UseNetworkSocket(true);
            }
            networkBuilder.SetProtocolRegister(protocolRegister);
            //this.protocolRegister = protocolRegister;
            return this;
        }

        public FacadeBuilder SetOutProcesserManager(JDataProcesserManager outProcesserManager)
        {
            if (networkBuilder == null)
            {
                UseNetworkSocket(true);
            }
            networkBuilder.SetOutDataProcesser(outProcesserManager);
            //this.outProcesserManager = outProcesserManager;
            return this;
        }

        public FacadeBuilder SetComingProcesserManager(JDataProcesserManager comingProcesserManager)
        {
            if (networkBuilder == null)
            {
                UseNetworkSocket(true);
            }
            networkBuilder.SetComingDataProcesser(comingProcesserManager);
            //this.comingProcesserManager = comingProcesserManager;
            return this;
        }

        public FacadeBuilder SetNetworkMessageHandler(BaseNetworkMessageHandler networkMessageHandler)
        {
            if (networkBuilder == null)
            {
                UseNetworkSocket(true);
            }
            networkBuilder.SetMessageHandler(networkMessageHandler);
            this.networkMessageHandler = networkMessageHandler;
            return this;
        }

        public FacadeBuilder SetTaskCompletionSourceManager(IJTaskCompletionSourceManager<IUnique> taskManager)
        {
            if (networkBuilder == null)
            {
                UseNetworkSocket(true);
            }
            networkBuilder.SetTaskManager(taskManager);
            //this.taskManager = taskManager;
            return this;
        }

        public FacadeBuilder SetGameAudioManager(IGameAudioManager gameAudioManager)
        {
            this.gameAudioManager = gameAudioManager;
            return this;
        }
    }
}
