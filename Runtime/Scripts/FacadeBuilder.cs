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

        IViewControllerManager viewControllerContainer;

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

            if(viewControllerContainer == null)
            {
                throw new System.Exception("ViewControllerContainer is required but not set in FacadeBuilder!");
            }

            var facade = new JFacade(uiManager, networkManager, assetsLoader, eventManager, sm, firstSceneState, context, viewControllerContainer);
    
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

        public FacadeBuilder SetViewControllerContainer(IViewControllerManager viewControllerContainer)
        {
            this.viewControllerContainer = viewControllerContainer;
            return this;
        }
    }
}
