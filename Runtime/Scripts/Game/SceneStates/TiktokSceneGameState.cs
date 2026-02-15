//using Adic;
//using Cysharp.Threading.Tasks;
//using Game.Common;
//using GameCommands;
//using JFramework.Game.View;
//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using static JFramework.Game.View.UIPanelMainMenu;


//namespace Tiktok
//{
//    public class TiktokSceneGameState : TiktokSceneLevelBaseState
//    {
//        [Inject("Game")]
//        protected ViewController[] gameControllers;

//        protected override async UniTask OnEnter(object arg)
//        {

//            await base.OnEnter(arg);

//            string levelUid = null;
//            if (arg != null)
//            {
//                var data = arg as CombatSceneTransitionArg;
//                if (data != null)
//                {
//                    var nodeUid = data.LevelNodeBusinessId;
//                    levelUid = tiktokConfigManager.GetLevelUid(nodeUid);
//                }
//                else
//                {
//                    levelUid = arg as string;
//                }
//            }

//            Debug.Log($"Enter TiktokSceneGameState with levelUid: {levelUid}");
//            var controllerArgs = new GameLevelBackgroundViewController.ControllerArgs();
//            controllerArgs.levelBusinessId = levelUid;
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

//        //to do: 和guidestate重复了，考虑抽取公共部分
//        List<UIMainEntrance> GetMainEntrances()
//        {
//            var entrances = new List<UIMainEntrance>();
//            entrances.Add(UIMainEntrance.EntranceDeploy);
//            entrances.Add(UIMainEntrance.EntranceCastle);

//            return entrances;
//        }

       


//        protected override SceneType GetSceneType()
//        {
//            return SceneType.SceneGame;
//        }

//        protected override string GetUISettingsName()
//        {
//            return "UISceneGameSettings";
//        }

//        protected override void OnInitializeVeiwControllers(ParallelLauncher viewControllers)
//        {
//            // 添加引导控制器
//            if (gameControllers == null || gameControllers.Length == 0)
//            {
//                return;
//            }
//            foreach (var controller in gameControllers)
//            {
//                viewControllers.Add(controller);
//            }
//        }

//        protected override ViewController[] GetControllers()
//        {
//            return levelControllers;
//        }

//        protected override string GetBGMClipName()
//        {
//            return tiktokConfigManager.GetLevelMusic("1");
//        }
//    }
//}
