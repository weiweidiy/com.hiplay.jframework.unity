//using Adic;
//using Adic.Container;
//using Cysharp.Threading.Tasks;
//using Game.Common;
//using Game.Share;
//using GameCommands;
//using JFramework;
//using JFramework.Game.View;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using static JFramework.Game.View.UIPanelMainMenu;
//using static Tiktok.UIGuideController;

//namespace Tiktok
//{
//    public class TiktokSceneGuideState : TiktokSceneLevelBaseState
//    {
//        [Inject("Guide")]
//        protected ViewController[] guideControllers;

//        [Inject]
//        protected GuideManager guideManager;

//        [Inject]
//        protected BaseGuideStep[] baseGuideSteps;

//        [Inject]
//        CurrentGuideStepModel currentGuideStepModel;
//        [Inject]
//        UIMainMenuController uiMainMenuController;


//        protected override async UniTask OnEnter(object arg)
//        {
//            InitGuideManager();

//            await base.OnEnter(arg);

//            var controllerArgs = new GameLevelBackgroundViewController.ControllerArgs();
//            controllerArgs.levelBusinessId = "1";
//            //进入副本
//            eventManager.Raise<GameLevelBackgroundViewController.Open>(controllerArgs);

//            //显示主菜单
//            var mainMenuControllerArgs = new UIMainMenuController.ControllerArgs
//            {
//                panelName = nameof(UIPanelMainMenu),
//                menuItems = GetMainEntrances()
//            };
//            //显示主菜单
//            eventManager.Raise<UIMainMenuController.Open>(mainMenuControllerArgs);
//        }

//        public override async UniTask OnExit()
//        {
//            await base.OnExit();
//            guideManager.Stop();
//        }

//        public override void AddListeners()
//        {
//            base.AddListeners();
//            eventManager.AddListener<CurrentGuideStepModel.EventUpdate>(OnStepUpdate);
//            eventManager.AddListener<UILevelNodeMenuController.UIControllerEventAttackClicked>(OnEnterLevelNodeConfirmed);
//            eventManager.AddListener<FormationDeployModel.EventUpdate>(OnDeployUpdate);
//            eventManager.AddListener<UIDialogController.EventDialogCompleted>(OnDialogComplete);
//        }



//        public override void RemoveListeners()
//        {
//            base.RemoveListeners();
//            eventManager.RemoveListener<CurrentGuideStepModel.EventUpdate>(OnStepUpdate);
//            eventManager.RemoveListener<UILevelNodeMenuController.UIControllerEventAttackClicked>(OnEnterLevelNodeConfirmed);
//            eventManager.RemoveListener<FormationDeployModel.EventUpdate>(OnDeployUpdate);
//            eventManager.RemoveListener<UIDialogController.EventDialogCompleted>(OnDialogComplete);
//        }


//        /// <summary>
//        /// 点击战斗开始，完成引导
//        /// </summary>
//        /// <param name="e"></param>
//        private void OnEnterLevelNodeConfirmed(UILevelNodeMenuController.UIControllerEventAttackClicked e)
//        {
//            var levelNodeBusinessId = e.Body as string;

//            if ((currentGuideStepModel.Data == null || currentGuideStepModel.Data == string.Empty) && levelNodeBusinessId == "1")
//            {
//                Debug.Log("通知服务器完成引导步骤 group1");

//                var dispatcher = container.GetCommandDispatcher();
//                dispatcher.Dispatch<CommandCompleteGuide>("1");
//                guideManager.Stop();
//            }

//            //if (levelNodeBusinessId == "3" && levelsModel.Get("3").Process == 0)
//            //{
//            //    Debug.Log("通知服务器完成引导步骤 group4");

//            //    var dispatcher = container.GetCommandDispatcher();
//            //    dispatcher.Dispatch<CommandCompleteGuide>("4");
//            //    guideManager.Stop();
//            //}

//        }

//        /// <summary>
//        /// 布阵更新，完成引导
//        /// </summary>
//        /// <param name="e"></param>
//        /// <exception cref="NotImplementedException"></exception>
//        private void OnDeployUpdate(FormationDeployModel.EventUpdate e)
//        {
//            var currentStepBusinessId = currentGuideStepModel.Data;
//            if (currentStepBusinessId == "1")
//            {
//                var dispatcher = container.GetCommandDispatcher();
//                dispatcher.Dispatch<CommandCompleteGuide>("2");
//                Debug.Log("通知服务器完成引导步骤 group2");
//            }

//            if(currentStepBusinessId == "2")
//            {
//                var dispatcher = container.GetCommandDispatcher();
//                dispatcher.Dispatch<CommandCompleteGuide>("3");
//                Debug.Log("通知服务器完成引导步骤 group3");
//            }          
//        }

//        private void OnDialogComplete(UIDialogController.EventDialogCompleted e)
//        {
//            var dialogGroupId = (int)e.Body;
//            Debug.Log("完成对话 " + dialogGroupId);
//            if (dialogGroupId == 5)
//            {
//                var dispatcher = container.GetCommandDispatcher();
//                //dispatcher.Dispatch<CommandCompleteGuide>("4");
//                //Debug.Log("通知服务器完成引导步骤 group4");

//                dispatcher.Dispatch<CommandSwitchScene>(SceneType.SceneCastle, default(object));
//            }
//        }



//        private void OnStepUpdate(CurrentGuideStepModel.EventUpdate e)
//        {
//            var stepBusinessId = e.Body as string;
//            Debug.Log($"引导步骤更新 {stepBusinessId}");
//            //eventManager.Raise<UIGuideController.Close>(null);
//        }

//        protected async void InitGuideManager()
//        {
//            guideManager.Stop();

//            var currentStepBusinessId = currentGuideStepModel.Data;
//            Debug.Log("当前引导步骤 " + currentStepBusinessId);
//            var steps = GetNextSteps(currentStepBusinessId);
//            if (steps.Count == 0)
//            {
//                Debug.Log("没有引导步骤");
//                return;
//            }
//            else
//            {
//                Debug.Log($"引导步骤数量 {steps.Count}");
//            }
//            guideManager.Enque(steps);

//            await guideManager.Start(new RunableExtraData()).AsUniTask();

//        }

//        List<UIMainEntrance> GetMainEntrances()
//        {
//            var currentStepBusinessId = currentGuideStepModel.Data;

//            var entrances = new List<UIMainEntrance>();

//            if (currentStepBusinessId != null && currentStepBusinessId != string.Empty && int.Parse(currentStepBusinessId) >= 2)
//                entrances.Add(UIMainEntrance.EntranceDeploy);

//            return entrances;
//        }

//        /// <summary>
//        /// 获取下一个引导步骤
//        /// </summary>
//        /// <param name="lastGuideBusinessId"></param>
//        /// <returns></returns>
//        List<BaseGuideStep> GetNextSteps(string lastGuideBusinessId)
//        {
//            string nextGuideBusinessId = "1";
//            if (lastGuideBusinessId != null && lastGuideBusinessId != string.Empty)
//            {
//                //从配置表获取下一个引导步骤
//                nextGuideBusinessId = tiktokConfigManager.GetNextGuideStepBusinessId(lastGuideBusinessId);
//            }
//            return CreateSetps(nextGuideBusinessId);
//        }

//        /// <summary>
//        /// 创建引导步骤
//        /// </summary>
//        /// <param name="guideBusinessId"></param>
//        /// <returns></returns>
//        List<BaseGuideStep> CreateSetps(string guideBusinessId)
//        {
//            var steps = new List<BaseGuideStep>();
//            switch (guideBusinessId)
//            {
//                case "1":
//                    {
//                        var trigger = container.Resolve<EventTriggerLevelNodeShow>();
//                        trigger.targetLevelNodeBusinessId = "1";
//                        trigger.targetLevelNodeProcess = 0;
//                        var step0 = container.Resolve<StepDialog>();
//                        step0.DialogGroupId = 1;
//                        var completeTrigger = container.Resolve<EventTriggerDialogCompleted>();
//                        completeTrigger.DialogGroupId = 1;
//                        step0.StartTriggers.Add(trigger);
//                        step0.CompleteTrigger = completeTrigger;



//                        var step1 = container.Resolve<StepMaskLevelNode>();
//                        var setp1CompleteTrigger = container.Resolve<EventTriggerLevelNodeClicked>();
//                        setp1CompleteTrigger.TargetLevelNodeBusinessId = "1";
//                        step1.CompleteTrigger = setp1CompleteTrigger;
//                        step1.TargetLevelNodeBusinessId = "1";

//                        var step2Trigger = container.Resolve<EventTriggerLevelNodeEntranceShow>();
//                        step2Trigger.TargetLevelNodeBusinessId = "1";
//                        step2Trigger.TargetLevelNodeProcess = 0;
//                        var step2CompleteTrigger = container.Resolve<EventTriggerLevelNodeConfirmed>();
//                        step2CompleteTrigger.TargetLevelNodeBusinessId = "1";
//                        var step2 = container.Resolve<StepMaskUIObject>();
//                        step2.StartTriggers.Add(step2Trigger);
//                        step2.CompleteTrigger = step2CompleteTrigger;
//                        steps.Add(step0);
//                        steps.Add(step1);
//                        steps.Add(step2);
//                    }
//                    break;
//                case "2":
//                    {
//                        //对话
//                        var trigger = container.Resolve<EventTriggerLevelNodeShow>();
//                        trigger.targetLevelNodeBusinessId = "2";
//                        trigger.targetLevelNodeProcess = 0;
//                        var completeTrigger = container.Resolve<EventTriggerDialogCompleted>();
//                        completeTrigger.DialogGroupId = 2;
//                        var step1 = container.Resolve<StepDialog>();
//                        step1.StartTriggers.Add(trigger);
//                        step1.CompleteTrigger = completeTrigger;
//                        step1.DialogGroupId = 2;
//                        steps.Add(step1);

//                        //显示获取武将
//                        var step2CombateTrigger = container.Resolve<EventTriggerRewardReceived>();
//                        var step2 = container.Resolve<StepShowSamuraiReward>();
//                        step2.samuraiBusinessId = "3";
//                        step2.CompleteTrigger = step2CombateTrigger;
//                        steps.Add(step2);

//                        //对话
//                        var step3CombateTrigger = container.Resolve<EventTriggerDialogCompleted>();
//                        step3CombateTrigger.DialogGroupId = 3;
//                        var step3 = container.Resolve<StepDialog>();
//                        step3.DialogGroupId = 3;
//                        step3.CompleteTrigger = step3CombateTrigger;
//                        steps.Add(step3);

//                        //显示解锁布阵
//                        var step4 = container.Resolve<StepUnlockSystem>();
//                        var step4CompleteTrigger = container.Resolve<EventTriggerMainMenuShow>();
//                        step4CompleteTrigger.entrance = UIMainEntrance.EntranceDeploy;
//                        step4.CompleteTrigger = step4CompleteTrigger;
//                        step4.SystemType = SystemType.Deploy;
//                        steps.Add(step4);

//                        //指向布阵入口
//                        var step5 = container.Resolve<StepMaskUIObject>();
//                        var step5CompleteTrigger = container.Resolve<EventTriggerDeployShow>();
//                        step5CompleteTrigger.guideIndex = 0;
//                        step5.CompleteTrigger = step5CompleteTrigger;
//                        //step1.MaskType = UIPanelGuideMask.MaskType.Parallelogram;
//                        steps.Add(step5);

//                        //指向上阵操作
//                        var step6 = container.Resolve<StepMaskUIObject>();
//                        step6.fingerType = FingerType.Swipe;
//                        var step6CombateTrigger = container.Resolve<EventTriggerDeployUpdate>();
//                        step6.CompleteTrigger = step6CombateTrigger;
//                        steps.Add(step6);

//                        //指向关闭
//                        var step7 = container.Resolve<StepMaskUIObject>();
//                        var step7CompleteTrigger = container.Resolve<EventTriggerDeployHide>();
//                        step7.CompleteTrigger = step7CompleteTrigger;
//                        steps.Add(step7);

//                        //指向节点2
//                        var step8 = container.Resolve<StepMaskLevelNode>();
//                        var setp8CompleteTrigger = container.Resolve<EventTriggerLevelNodeClicked>();
//                        setp8CompleteTrigger.TargetLevelNodeBusinessId = "2";
//                        step8.CompleteTrigger = setp8CompleteTrigger;
//                        step8.TargetLevelNodeBusinessId = "2";
//                        steps.Add(step8);

//                        //指向节点确认按钮
//                        var step9Trigger = container.Resolve<EventTriggerLevelNodeEntranceShow>();
//                        step9Trigger.TargetLevelNodeBusinessId = "2";
//                        step9Trigger.TargetLevelNodeProcess = 0;
//                        var step9 = container.Resolve<StepMaskUIObject>();
//                        var step9CompleteTrigger = container.Resolve<EventTriggerLevelNodeConfirmed>();
//                        step9CompleteTrigger.TargetLevelNodeBusinessId = "2";
//                        step9.StartTriggers.Add(step9Trigger);
//                        step9.CompleteTrigger = step9CompleteTrigger;
//                        steps.Add(step9);
//                    }
//                    break;
//                case "3":
//                    {
//                        var trigger = container.Resolve<EventTriggerLevelNodeShow>();
//                        trigger.targetLevelNodeBusinessId = "3";
//                        trigger.targetLevelNodeProcess = 0;
//                        var step0 = container.Resolve<StepDialog>();
//                        step0.DialogGroupId = 4;
//                        var completeTrigger = container.Resolve<EventTriggerDialogCompleted>();
//                        completeTrigger.DialogGroupId = 4;
//                        step0.StartTriggers.Add(trigger);
//                        step0.CompleteTrigger = completeTrigger;
//                        steps.Add(step0);

//                        //指向编队入口按钮
//                        var step1 = container.Resolve<StepMaskUIObject>();
//                        var stpe1StartTrigger = container.Resolve<EventTriggerMainMenuGet>();
//                        stpe1StartTrigger.entrance = UIMainEntrance.EntranceDeploy;
//                        var step1CompleteTrigger = container.Resolve<EventTriggerDeployShow>();
//                        step1CompleteTrigger.guideIndex = 1;
//                        step1.StartTriggers.Add(stpe1StartTrigger);
//                        step1.CompleteTrigger = step1CompleteTrigger;
//                        steps.Add(step1);

//                        //指向上阵操作
//                        var step2 = container.Resolve<StepMaskUIObject>();
//                        step2.fingerType = FingerType.Swipe;
//                        var step2CombateTrigger = container.Resolve<EventTriggerDeployUpdate>();
//                        step2.CompleteTrigger = step2CombateTrigger;
//                        steps.Add(step2);

//                        //指向关闭
//                        var step3 = container.Resolve<StepMaskUIObject>();
//                        var step3CompleteTrigger = container.Resolve<EventTriggerDeployHide>();
//                        step3.CompleteTrigger = step3CompleteTrigger;
//                        steps.Add(step3);

//                        //指向节点3
//                        var step8 = container.Resolve<StepMaskLevelNode>();
//                        var setp8CompleteTrigger = container.Resolve<EventTriggerLevelNodeClicked>();
//                        setp8CompleteTrigger.TargetLevelNodeBusinessId = "3";
//                        step8.CompleteTrigger = setp8CompleteTrigger;
//                        step8.TargetLevelNodeBusinessId = "3";
//                        steps.Add(step8);

//                        //指向节点确认按钮
//                        var step9Trigger = container.Resolve<EventTriggerLevelNodeEntranceShow>();
//                        step9Trigger.TargetLevelNodeBusinessId = "3";
//                        step9Trigger.TargetLevelNodeProcess = 0;
//                        var step9 = container.Resolve<StepMaskUIObject>();
//                        var step9CompleteTrigger = container.Resolve<EventTriggerLevelNodeConfirmed>();
//                        step9CompleteTrigger.TargetLevelNodeBusinessId = "3";
//                        step9.StartTriggers.Add(step9Trigger);
//                        step9.CompleteTrigger = step9CompleteTrigger;
//                        steps.Add(step9);

//                    }
//                    break;
//                case "4":
//                    {
//                        var trigger = container.Resolve<EventTriggerLevelNodeShow>();
//                        trigger.targetLevelNodeBusinessId = "3";
//                        trigger.targetLevelNodeProcess = 1;
//                        var trigger2 = container.Resolve<EventTriggerLevelNodeUpdate>();
//                        trigger2.targetLevelNodeBusinessId = "3";
//                        trigger2.targetLevelNodeProcess = 1;
//                        var step0 = container.Resolve<StepDialog>();
//                        step0.DialogGroupId = 5;
//                        var completeTrigger = container.Resolve<EventTriggerDialogCompleted>();
//                        completeTrigger.DialogGroupId = 5;
//                        step0.StartTriggers.Add(trigger);
//                        step0.StartTriggers.Add(trigger2);
//                        step0.CompleteTrigger = completeTrigger;
//                        steps.Add(step0);


//                    }
//                    break;
//            }
//            return steps;
//        }


//        protected override string GetBGMClipName()
//        {
//            return tiktokConfigManager.GetLevelMusic("1");
//        }

//        protected override ViewController[] GetControllers()
//        {
//            return levelControllers;
//        }

//        protected override SceneType GetSceneType()
//        {
//            return SceneType.SceneGuide;
//        }

//        protected override string GetUISettingsName()
//        {
//            return "UISceneGuideSettings";
//        }


//        protected override void OnInitializeVeiwControllers(ParallelLauncher viewControllers)
//        {
//            // 添加引导控制器
//            if (guideControllers == null || guideControllers.Length == 0)
//            {
//                return;
//            }
//            foreach (var controller in guideControllers)
//            {
//                viewControllers.Add(controller);
//            }
//        }


//    }
//}
