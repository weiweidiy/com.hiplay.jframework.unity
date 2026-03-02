using JFramework.Game;

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

        IModelManager modelManager;

        IControllerManager controllerManager;

        IGameObjectPool gameObjectPool;

        IGameObjectManager gameObjectManager;

        IHttpRequest httpRequest;

        IDataConverter dataConverter;

        IJConfigManager configManager;

        IConfigLoader configLoader;

        ISpriteManager spriteManager;

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

            if (dataConverter == null)
            {
                dataConverter = new DefaultDataConverter();
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

            if(spriteManager == null)
            {
                spriteManager = new DefaultSpriteManager(assetsLoader);
            }

            var facade = new JFacade(uiManager, networkManager, assetsLoader, eventManager, sm, firstSceneState, context
                    , gameObjectManager, modelManager, viewControllerManager, controllerManager, httpRequest, configManager, spriteManager);

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

        public FacadeBuilder SetModelManager(IModelManager modelManager)
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
    }
}
