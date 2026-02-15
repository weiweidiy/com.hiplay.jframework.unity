//using Adic;
//using Cysharp.Threading.Tasks;
//using Game.Common;
//using GameCommands;
//using JFramework;
//using JFramework.Game.View;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using UnityEngine;
//using static JFramework.Game.View.UIPanelMainMenu;

//namespace Tiktok
//{
//    public class TiktokSceneCastleState : BaseSceneState
//    {
//        [Inject("Castle")]
//        ViewController[] controllers;

//        [Inject]
//        TiktokGameDataManager gameDataManager;

//        [Inject]
//        SamuraisModel samuraisModel;

//        [Inject]
//        FormationModel formationModel;

//        [Inject]
//        LevelsModel levelsModel;

//        [Inject]
//        FormationDeployModel formationDeployModel;


//        protected override async UniTask OnEnter(object arg)
//        {
//            await base.OnEnter(arg);

//            var bgControllerArgs = new CastleBackgroundViewController.ControllerArgs()
//            {
//                //panelName = "CastleBackgroundView"
//            };
//            eventManager.Raise<CastleBackgroundViewController.Open>(bgControllerArgs);


//            var mainMenuControllerArgs = new UIMainMenuController.ControllerArgs()
//            {
//                panelName = nameof(UIPanelMainMenu),
//                menuItems = GetMainMenuEntrances()
//            };
//            eventManager.Raise<UIMainMenuController.Open>(mainMenuControllerArgs);
//        }

//        public override void AddListeners()
//        {
//            base.AddListeners();

//            eventManager.AddListener<CastleBuildingsViewController.EventBuildingClicked>(OnBuildingClicked);
//            eventManager.AddListener<CastleBackgroundViewController.EventBuildingCreateClicked>(OnBuildingCreateClicked);
//            eventManager.AddListener<UIBuildingMenuController.EventBuildingPopupMenuClicked>(OnBuildingMenuClicked);
//            eventManager.AddListener<UIDeployController.EventDeployUnit>(OnDeploy);
//            eventManager.AddListener<UIDeployController.EventChangeFormation>(OnChangeFormation);
//            eventManager.AddListener<UILevelListController.EventCellClicked>(OnLevelCellClicked);
//            eventManager.AddListener<UIMainMenuController.EventEnteranceClicked>(OnMainMenuClicked);
//        }



//        public override void RemoveListeners()
//        {
//            base.RemoveListeners();

//            eventManager.RemoveListener<CastleBuildingsViewController.EventBuildingClicked>(OnBuildingClicked);
//            eventManager.RemoveListener<CastleBackgroundViewController.EventBuildingCreateClicked>(OnBuildingCreateClicked);
//            eventManager.RemoveListener<UIBuildingMenuController.EventBuildingPopupMenuClicked>(OnBuildingMenuClicked);
//            eventManager.RemoveListener<UIDeployController.EventDeployUnit>(OnDeploy);
//            eventManager.RemoveListener<UIDeployController.EventChangeFormation>(OnChangeFormation);
//            eventManager.RemoveListener<UILevelListController.EventCellClicked>(OnLevelCellClicked);
//            eventManager.RemoveListener<UIMainMenuController.EventEnteranceClicked>(OnMainMenuClicked);

//        }

//        /// <summary>
//        /// 建筑按钮被点击了
//        /// </summary>
//        /// <param name="e"></param>
//        private void OnBuildingClicked(CastleBuildingsViewController.EventBuildingClicked e)
//        {
//            var controllerArgs = e.Body as CastleBuildingsViewController.ControllerArgs;
//            var goTarget = controllerArgs.goTarget;
//            var targetBusinessId = controllerArgs.targetBusinessId;

//            var menuTypes = GetUIBuildingMenus(targetBusinessId);

//            var controllerData = new UIBuildingMenuController.ControllerArgs()
//            {
//                panelName = nameof(UIPanelBuildingMenus),
//                menuTypes = menuTypes,
//                worldPosition = goTarget.transform.position,
//                businessId = targetBusinessId,

//            };
//            eventManager.Raise<UIBuildingMenuController.Open>(controllerData);
//        }

//        /// <summary>
//        /// 创建建筑按钮被点击了
//        /// </summary>
//        /// <param name="e"></param>
//        /// <exception cref="NotImplementedException"></exception>
//        private void OnBuildingCreateClicked(CastleBackgroundViewController.EventBuildingCreateClicked e)
//        {
//            var args = e.Body as CastleBackgroundViewController.ControllerArgs;
//            Debug.Log("点击了创建建筑按钮 !" + args.targetBusinessId);

//            var dispatcher = container.GetCommandDispatcher();
//            dispatcher.Dispatch<CommandCreateBuilding>(args.targetBusinessId);
//        }

//        /// <summary>
//        /// 主菜单入口被点击了
//        /// </summary>
//        /// <param name="e"></param>
//        /// <exception cref="ArgumentOutOfRangeException"></exception>
//        private void OnMainMenuClicked(UIMainMenuController.EventEnteranceClicked e)
//        {
//            var entrance = (UIMainEntrance)e.Body;
//            switch (entrance)
//            {
//                case UIMainEntrance.EntranceSamuraisList: //to do: 移去gamestate里
//                    {
//                        OpenSamuraiList();
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
//                default:
//                    throw new ArgumentOutOfRangeException("没有实现btnclick " + entrance);
//            }
//        }

//        /// <summary>
//        /// 建筑菜单被点击了
//        /// </summary>
//        /// <param name="e"></param>
//        /// <exception cref="NotImplementedException"></exception>
//        private void OnBuildingMenuClicked(UIBuildingMenuController.EventBuildingPopupMenuClicked e)
//        {
//            var controllerData = e.Body as UIBuildingMenuController.ControllerArgs;
//            if (controllerData == null) return;

//            var businessId = controllerData.businessId;
//            var clickedMenu = controllerData.clickedMenu;
//            switch (clickedMenu)
//            {
//                case UIPanelBuildingMenus.UIBuildingMenu.Upgrade:
//                    {
//                        var dispatcher = container.GetCommandDispatcher();
//                        dispatcher.Dispatch<CommandUpgradeBuilding>(businessId);
//                    }
//                    break;
//                case UIPanelBuildingMenus.UIBuildingMenu.Complete:
//                    {
//                        var dispatcher = container.GetCommandDispatcher();
//                        dispatcher.Dispatch<CommandCompleteBuildingUpgrade>(businessId);
//                    }
//                    break;
//                case UIPanelBuildingMenus.UIBuildingMenu.Detail:
//                    {
//                        //var controllerArgs = new UIBuildingDetailController.ControllerArgs()
//                        //{
//                        //    panelName = nameof(UIPanelBuildingDetail),
//                        //    businessId = businessId,
//                        //};
//                        //eventManager.Raise<UIBuildingDetailController.Open>(controllerArgs);
//                    }
//                    break;
//                case UIPanelBuildingMenus.UIBuildingMenu.Enter:
//                    {
//                        switch (businessId)
//                        {
//                            case "1":
//                                {
//                                    OpenSamuraiList();
//                                }

//                                break;
//                            case "2":
//                                {
//                                    OpenHpPool();
//                                }
//                                break;
//                            default:
//                                Debug.Log("点击了未实现的建筑进入菜单 " + businessId);
//                                break;
//                        }

//                    }
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException("没有实现菜单类型 " + clickedMenu);
//            }
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

//        /// <summary>
//        /// 某个关卡入口被点击了
//        /// </summary>
//        /// <param name="e"></param>
//        private void OnLevelCellClicked(UILevelListController.EventCellClicked e)
//        {
//            var levelUid = (string)e.Body;

//            var dispatcher = container.GetCommandDispatcher();
//            dispatcher.Dispatch<CommandSwitchScene>(SceneType.SceneGame, levelUid);
//        }

//        #region 抽象方法实现
//        protected override string GetBGMClipName()
//        {
//            return tiktokConfigManager.GetCastleMusic();
//        }

//        protected override ViewController[] GetControllers()
//        {
//            return controllers;
//        }

//        protected override SceneType GetSceneType()
//        {
//            return SceneType.SceneCastle;
//        }

//        protected override string GetUISettingsName()
//        {
//            return "UISceneCastleSettings";
//        }
//        #endregion

//        /// <summary>
//        /// 获取主菜单入口列表
//        /// </summary>
//        /// <returns></returns>
//        List<UIMainEntrance> GetMainMenuEntrances()
//        {
//            return new List<UIMainEntrance>() { UIMainEntrance.EntranceDeploy
//                //, UIMainEntrance.EntranceSamuraisList
//                , UIMainEntrance.EntranceLevel};
//        }

//        /// <summary>
//        /// 获取指定建筑的菜单列表
//        /// </summary>
//        /// <param name="businissId"></param>
//        /// <returns></returns>
//        List<UIPanelBuildingMenus.UIBuildingMenu> GetUIBuildingMenus(string businissId)
//        {
//            var result = new List<UIPanelBuildingMenus.UIBuildingMenu>();
//            var isUpgrading = gameDataManager.IsBuildingUpgrading(businissId);
//            //根据是否在升级中，显示不同的菜单
//            if (isUpgrading)
//                result.Add(UIPanelBuildingMenus.UIBuildingMenu.Complete);
//            else
//                result.Add(UIPanelBuildingMenus.UIBuildingMenu.Upgrade);

//            //根据建筑类型，显示不同的菜单
//            result.Add(UIPanelBuildingMenus.UIBuildingMenu.Enter);

//            // 详细菜单暂时都加上
//            result.Add(UIPanelBuildingMenus.UIBuildingMenu.Detail);

//            return result;
//        }

//        void OpenSamuraiList()
//        {
//            var controllerArgs = new UISamuraisListController.ControllerArgs();
//            controllerArgs.panelName = nameof(UIPanelSamuraisList);
//            controllerArgs.samuraiDTOs = samuraisModel.GetAll();
//            eventManager.Raise<UISamuraisListController.Open>(controllerArgs);
//        }

//        void OpenHpPool()
//        {
//            var controllerArgs = new UIHpPoolController.ControllerArgs();
//            controllerArgs.panelName = nameof(UIPanelHpPool);
//            eventManager.Raise<UIHpPoolController.Open>(controllerArgs);
//        }
//    }
//}
