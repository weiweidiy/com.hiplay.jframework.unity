//using Cysharp.Threading.Tasks;
//using System;

////using Game.Share;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace JFramework.Unity
//{
//    public enum DefaultSceneType
//    {
//        None = 0,
//        Init,
//        SceneLogin,
//        SceneGuide,
//        SceneGame,
//        SceneCombat,
//        SceneCastle,
//    }

//    public enum DefaultSceneSMTrigger
//    {
//        Login,
//        Game,
//        Guide,
//        Combat,
//        Castle,
//    }

//    /// <summary>
//    /// 状态机上下文，持有状态机实例，可以在状态之间传递数据，onEnter时传入
//    /// </summary>
//    public class GameContext
//    {
//        public DefaultSceneSM sm;
//    }

//    public abstract class DefaultBaseSceneState : BaseSceneState<GameContext, DefaultSceneType>
//    {
//        public DefaultBaseSceneState(IAssetsLoader assetsLoader, IJUIManager uiManager, EventManager eventManager) : base(assetsLoader)
//        {
//        }
//    }


//    public class DefaultSceneSM : BaseSMAsync<GameContext, DefaultBaseSceneState, DefaultSceneSMTrigger>
//    {
//        DefaultSceneInitState initState;
//        DefaultSceneLoginState loginState;
//        //TiktokSceneGuideState guideState;
//        //TiktokSceneCastleState castleState;
//        //TiktokSceneGameState gameState;
//        //TiktokSceneCombatState combatState;

//        //如果需要在状态切换时传递参数，可以在这里定义对应的SMConfig
//        //SMConfig combatConfig;
//        //SMConfig gameConfig;
//        //SMConfig castleConfig;
//        //SMConfig guideConfig;

//        public DefaultSceneSM(GameContext context) : base(context)
//        {
//            context.sm = this;
//        }

//        protected override List<DefaultBaseSceneState> GetAllStates()
//        {
//            var states = new List<DefaultBaseSceneState>();

//            initState = new DefaultSceneInitState(AssetsLoader, UIManager, EventManager);
//            if (initState == null)
//                throw new Exception("Resolve DefaultSceneInitState is null");

//            loginState = new DefaultSceneLoginState(AssetsLoader, UIManager, EventManager);
//            if (loginState == null)
//                throw new Exception("Resolve TiktokSceneMenuState is null");

//            //guideState = container.Resolve<TiktokSceneGuideState>();
//            //if (guideState == null)
//            //    throw new Exception("Resolve TiktokSceneGuideState is null");

//            //castleState = container.Resolve<TiktokSceneCastleState>();
//            //if (castleState == null)
//            //    throw new Exception("Resolve TiktokSceneCastleState is null");

//            //gameState = container.Resolve<TiktokSceneGameState>();
//            //if (gameState == null)
//            //    throw new Exception("Resolve TiktokSceneGameState is null");

//            //combatState = container.Resolve<TiktokSceneCombatState>();
//            //if (combatState == null)
//            //    throw new Exception("Resolve TiktokSceneCombatState is null");

//            states.Add(initState);
//            states.Add(loginState);
//            //states.Add(guideState);
//            //states.Add(castleState);
//            //states.Add(gameState);
//            //states.Add(combatState);
//            var additionalStates = GetAdditionalStates();
//            if(additionalStates != null)
//                states.AddRange(additionalStates);

//            return states;
//        }

//        /// <summary>
//        /// 子类可以通过重写这个方法来添加额外的状态，避免修改父类代码，符合开闭原则
//        /// </summary>
//        /// <returns></returns>
//        protected virtual List<DefaultBaseSceneState> GetAdditionalStates()
//        {
//            return null;
//        }

//        protected override Dictionary<string, SMConfig> GetConfigs()
//        {
//            var configs = new Dictionary<string, SMConfig>();

//            var initName = initState.Name;
//            var initConfig = new SMConfig();
//            initConfig.state = initState;
//            initConfig.dicPermit = new Dictionary<DefaultSceneSMTrigger, DefaultBaseSceneState>();
//            initConfig.dicPermit.Add(DefaultSceneSMTrigger.Login, loginState);
//            configs.Add(initName, initConfig);


//            var loginName = loginState.Name;
//            var loginConfig = new SMConfig();
//            loginConfig.state = loginState;
//            loginConfig.dicPermit = new Dictionary<DefaultSceneSMTrigger, DefaultBaseSceneState>();
//            //loginConfig.dicPermit.Add(DefaultSceneSMTrigger.Game, gameState); // 可以进入游戏状态
//            //loginConfig.dicPermit.Add(DefaultSceneSMTrigger.Guide, guideState); // 可以进入新手引导状态
//            //loginConfig.dicPermit.Add(DefaultSceneSMTrigger.Castle, castleState); // 可以进入城堡状态
//            configs.Add(loginName, loginConfig);


//            //var guideName = guideState.Name;
//            //guideConfig = new SMConfig();
//            //guideConfig.state = guideState;
//            //guideConfig.dicPermit = new Dictionary<DefaultSceneSMTrigger, BaseSceneState>();
//            //guideConfig.dicPermit.Add(DefaultSceneSMTrigger.Castle, castleState); // 可以进入城堡状态
//            //guideConfig.dicPermit.Add(DefaultSceneSMTrigger.Combat, combatState); // 可以进入战斗状态
//            //guideConfig.parameter = machine.SetTriggerParameters<object>(DefaultSceneSMTrigger.Guide);
//            //configs.Add(guideName, guideConfig);


//            //var castleName = castleState.Name;
//            //castleConfig = new SMConfig();
//            //castleConfig.state = castleState;
//            //castleConfig.dicPermit = new Dictionary<DefaultSceneSMTrigger, BaseSceneState>();
//            //castleConfig.dicPermit.Add(DefaultSceneSMTrigger.Game, gameState); // 可以进入游戏状态
//            //castleConfig.dicPermit.Add(DefaultSceneSMTrigger.Combat, combatState); // 可以进入战斗状态
//            //castleConfig.parameter = machine.SetTriggerParameters<object>(DefaultSceneSMTrigger.Castle);
//            //configs.Add(castleName, castleConfig);


//            //var gameName = gameState.Name;
//            //gameConfig = new SMConfig();
//            //gameConfig.state = gameState;
//            //gameConfig.dicPermit = new Dictionary<DefaultSceneSMTrigger, BaseSceneState>();
//            //gameConfig.dicPermit.Add(DefaultSceneSMTrigger.Combat, combatState); // 可以进入战斗状态
//            //gameConfig.dicPermit.Add(DefaultSceneSMTrigger.Castle, castleState); // 可以进入城堡状态
//            //gameConfig.parameter = machine.SetTriggerParameters<object>(DefaultSceneSMTrigger.Game);
//            //configs.Add(gameName, gameConfig);

//            //var combatName = combatState.Name;
//            //combatConfig = new SMConfig();
//            //combatConfig.state = combatState;
//            //combatConfig.dicPermit = new Dictionary<DefaultSceneSMTrigger, BaseSceneState>();
//            //combatConfig.dicPermit.Add(DefaultSceneSMTrigger.Game, gameState); // 可以返回到游戏状态
//            //combatConfig.dicPermit.Add(DefaultSceneSMTrigger.Castle, castleState); // 可以返回到城堡状态
//            //combatConfig.dicPermit.Add(DefaultSceneSMTrigger.Guide, guideState); // 可以返回到新手引导状态
//            //combatConfig.parameter = machine.SetTriggerParameters<object>(DefaultSceneSMTrigger.Combat);
//            //configs.Add(combatName, combatConfig);


//            return  GetAdditionalConfigs(configs);
//        }

//        /// <summary>
//        /// 子类可以通过重写这个方法来添加额外的状态配置，避免修改父类代码，符合开闭原则
//        /// </summary>
//        /// <param name="config"></param>
//        /// <returns></returns>
//        protected virtual Dictionary<string, SMConfig> GetAdditionalConfigs(Dictionary<string, SMConfig> config)
//        {
//            return config;
//        }


//        public override Task SwitchToState(string stateName)
//        {
//            if (stateName == DefaultSceneType.SceneLogin.ToString())
//                return SwitchToLogin().AsTask();

//            return SwitchToOtherState(stateName);
//        }

//        /// <summary>
//        /// 子类可以通过重写这个方法来添加额外的状态切换逻辑，避免修改父类代码，符合开闭原则
//        /// </summary>
//        /// <param name="stateName"></param>
//        /// <returns></returns>
//        protected virtual Task SwitchToOtherState(string stateName)
//        {
//            return Task.CompletedTask;
//        }

//        UniTask SwitchToLogin()
//        {
//            return machine.FireAsync(DefaultSceneSMTrigger.Login);
//        }

//        //public UniTask SwitchToGame(object none)
//        //{
//        //    return machine.FireAsync(gameConfig.parameter, none);
//        //}

//        //public UniTask SwitchToCombat(object responesFight)
//        //{
//        //    return machine.FireAsync(combatConfig.parameter, responesFight);
//        //}

//        //public UniTask SwitchToGuide(object none)
//        //{
//        //    return machine.FireAsync(guideConfig.parameter, none);
//        //}


//        //public UniTask SwitchToCastle(object none)
//        //{
//        //    return machine.FireAsync(castleConfig.parameter, none);
//        //}


//    }
//}
