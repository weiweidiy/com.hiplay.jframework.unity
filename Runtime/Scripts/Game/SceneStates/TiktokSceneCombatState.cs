//using Adic;
//using Cysharp.Threading.Tasks;
//using Game.Common;
//using GameCommands;

//namespace Tiktok
//{
//    public class TiktokSceneCombatState : BaseSceneState
//    {
//        [Inject("Combat")]
//        ViewController[] controllers;

//        protected override async UniTask OnEnter(object arg)
//        {
//            await base.OnEnter(arg);
            
//            var controllerArgs = new CombatViewController.ControllerArgs()
//            {
//                arg = (CombatSceneTransitionArg)arg,
//                //panelName = "CombatView"
//            };
//            eventManager.Raise<CombatViewController.Open>(controllerArgs);
//        }

//        public override void AddListeners()
//        {
//            base.AddListeners();
//            eventManager.AddListener<EventExitCombat>(OnCombatExit);
//        }

//        public override void RemoveListeners()
//        {
//            base.RemoveListeners();
//            eventManager.RemoveListener<EventExitCombat>(OnCombatExit);
//        }

//        protected override string GetUISettingsName()
//        {
//            return "UISceneCombatSettings";
//        }

//        private void OnCombatExit(EventExitCombat e)
//        {
//            var dispatcher = container.GetCommandDispatcher();
//            dispatcher.Dispatch<CommandFightReturnBack>(arg);
//        }

//        protected override ViewController[] GetControllers()
//        {
//            return controllers;
//        }

//        protected override SceneType GetSceneType()
//        {
//            return SceneType.SceneCombat;
//        }

//        protected override string GetBGMClipName()
//        {
//            var responseFight = (this.arg as CombatSceneTransitionArg).FightResponse;
//            return tiktokConfigManager.GetCombatMusic(responseFight.LevelNodeBusinessId);
//        }
//    }
//}
