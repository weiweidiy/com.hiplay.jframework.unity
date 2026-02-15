//using Adic;
//using Adic.Container;
//using Cysharp.Threading.Tasks;
//using Game.Common;
//using Game.Share;
//using GameCommands;
//using JFramework;
//using JFramework.Game.View;
//using System;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using static JFramework.Game.View.UIPanelMainMenu;

//namespace Tiktok
//{
//    public abstract class TiktokSceneLevelBaseState : BaseSceneState
//    {
//        [Inject("Level")]
//        protected ViewController[] levelControllers;

//        [Inject]
//        protected LevelsModel levelsModel;

//        [Inject]
//        protected SamuraisModel samuraisModel;

//        [Inject]
//        protected FormationModel formationModel;

//        [Inject]
//        protected FormationDeployModel formationDeployModel;

//        protected override async UniTask OnEnter(object arg)
//        {
//            await base.OnEnter(arg);

//            //播放战斗奖励
//            if (arg != null)
//            {
//                var data = arg as CombatSceneTransitionArg;
//                if (data != null)
//                    PlayFightReward(data.FightResponse);
//            }
//        }

//        public override void AddListeners()
//        {
//            base.AddListeners();
//            eventManager.AddListener<GameLevelNodeViewController.EventLevelNodeClicked>(OnLevelNodeClicked);   
//            eventManager.AddListener<UILevelNodeMenuController.UIControllerEventAttackClicked>(OnEnterLevelNodeMenuAttack);

//            eventManager.AddListener<UIDeployController.EventDeployUnit>(OnDeploy);
//            eventManager.AddListener<UIDeployController.EventChangeFormation>(OnChangeFormation);

//            eventManager.AddListener<UIMainMenuController.EventEnteranceClicked>(OnMainMenuClicked);
//        }

//        public override void RemoveListeners()
//        {
//            base.RemoveListeners();
//            eventManager.RemoveListener<GameLevelNodeViewController.EventLevelNodeClicked>(OnLevelNodeClicked);
//            eventManager.RemoveListener<UILevelNodeMenuController.UIControllerEventAttackClicked>(OnEnterLevelNodeMenuAttack);

//            eventManager.RemoveListener<UIDeployController.EventDeployUnit>(OnDeploy);
//            eventManager.RemoveListener<UIDeployController.EventChangeFormation>(OnChangeFormation);

//            eventManager.RemoveListener<UIMainMenuController.EventEnteranceClicked>(OnMainMenuClicked);
//        }

//        private void OnMainMenuClicked(UIMainMenuController.EventEnteranceClicked e)
//        {
//            var entrance = (UIMainEntrance)e.Body;
//            switch (entrance)
//            {
//                case UIMainEntrance.EntranceSamuraisList: //to do: 移去gamestate里
//                    {
//                        var controllerArgs = new UISamuraisListController.ControllerArgs();
//                        controllerArgs.panelName = nameof(UIPanelSamuraisList);
//                        controllerArgs.samuraiDTOs = samuraisModel.GetAll();
//                        eventManager.Raise<UISamuraisListController.Open>(controllerArgs);
//                    }

//                    break;
//                case UIMainEntrance.EntranceDrawSamurai:
//                    {
//                        var controllerArgs = new UIKachaController.ControllerArgs();
//                        controllerArgs.panelName = nameof(UIPanelKacha);
//                        //打开关卡界面
//                        eventManager.Raise<UIKachaController.Open>(controllerArgs);
//                    }

//                    break;
//                case UIMainEntrance.EntranceDeploy:
//                    {
//                        var controllerArgs = new UIDeployController.ControllerArgs();
//                        controllerArgs.panelName = nameof(UIPanelDeploy);
//                        controllerArgs.formationDeployDTOs = formationDeployModel.GetAll();
//                        controllerArgs.samuraiDTOs = samuraisModel.GetAll();
//                        controllerArgs.formationDTOs = formationModel.GetAll();
//                        eventManager.Raise<UIDeployController.Open>(controllerArgs);
//                    }
//                    break;
//                case UIMainEntrance.EntranceLevel:
//                    {
//                        Debug.Log("点击了关卡列表按钮EntranceLevel");
//                        var controllerArgs = new UILevelListController.ControllerArgs();
//                        controllerArgs.panelName = nameof(UIPanelLevelList);
//                        controllerArgs.levelNodeDTOs = levelsModel.GetAll();
//                        //打开关卡界面
//                        eventManager.Raise<UILevelListController.Open>(controllerArgs);
//                    }
//                    break;
//                case UIMainEntrance.EntranceCastle:
//                    {
//                        Debug.Log("点击了城堡按钮EntranceCastle");
//                        var dispatcher = container.GetCommandDispatcher();
//                        dispatcher.Dispatch<CommandSwitchScene>(SceneType.SceneCastle, default(object));

//                    }
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException("没有实现btnclick " + entrance);
//            }
//        }


//        /// <summary>
//        /// 播放战斗奖励流程
//        /// </summary>
//        /// <param name="respFight"></param>
//        protected void PlayFightReward(ResponseFight respFight)
//        {
//            if( respFight.WinRewardDTO != null && respFight.WinRewardDTO.Currencies != null && respFight.WinRewardDTO.Currencies.Count > 0)
//            {
//                var controllerArgs = new GameLevelCombatResultController.ControllerArgs()
//                {
//                    RespFight = respFight
//                };
//                eventManager.Raise<GameLevelCombatResultController.Open>(controllerArgs);
//            }
//        }

//        /// <summary>
//        /// 关卡节点被点击了
//        /// </summary>
//        /// <param name="e"></param>
//        private void OnLevelNodeClicked(GameLevelNodeViewController.EventLevelNodeClicked e)
//        {
//            var panelData = e.Body as GameLevelNodeViewController.PanelData;
//            var levelNodeBusinessId = panelData.businessId;
//            var dto = levelsModel.Get(levelNodeBusinessId);
//            var menuPanelData = new UILevelNodeMenuController.ControllerArgs()
//            {
//                panel = null,
//                dto = dto,
//                worldPosition = panelData.nodeView.transform.position,
//                panelName = nameof(UIPanelLevelNodeMenus)
//            };
//            eventManager.Raise<UILevelNodeMenuController.Open>(menuPanelData);
//        }

//        /// <summary>
//        /// 确认开始战斗
//        /// </summary>
//        /// <param name="e"></param>
//        private void OnEnterLevelNodeMenuAttack(UILevelNodeMenuController.UIControllerEventAttackClicked e)
//        {
//            var levelNodeBusinessId = e.Body as string;
//            var dispatcher = container.GetCommandDispatcher();
//            dispatcher.Dispatch<CommandFight>(levelNodeBusinessId, GetSceneType());
//        }

//        /// <summary>
//        /// 布阵
//        /// </summary>
//        /// <param name="e"></param>
//        private void OnDeploy(UIDeployController.EventDeployUnit e)
//        {
//            var data = e.Body as UIDeployDragableScrollerUnitView.DropData;
//            var dispatcher = container.GetCommandDispatcher();
//            dispatcher.Dispatch<CommandDeploy>(data.formationIndex, data.samuraiId);

//            Debug.Log($"OnDeploy samuraiId: {data.samuraiId} + formationIndex: {data.formationIndex}");
//        }

//        /// <summary>
//        /// 切换阵型了
//        /// </summary>
//        /// <param name="e"></param>
//        /// <exception cref="NotImplementedException"></exception>
//        private void OnChangeFormation(UIDeployController.EventChangeFormation e)
//        {
//            var formationBusinessId = (string)e.Body;
//            Debug.Log("OnChangeFormation " + formationBusinessId);
//            var dispatcher = container.GetCommandDispatcher();
//            dispatcher.Dispatch<CommandChangeFormation>(formationBusinessId);
//        }

//    }
//}
