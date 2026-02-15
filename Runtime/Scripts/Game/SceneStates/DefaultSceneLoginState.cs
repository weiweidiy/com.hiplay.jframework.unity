
//using Cysharp.Threading.Tasks;
//using Game.Common;

//using JFramework;

//using JFramework.Game;

//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//namespace JFramework.Unity
//{
//    public class DefaultSceneLoginState : DefaultBaseSceneState
//    {

//        ViewController[] controllers;

//        public DefaultSceneLoginState(IAssetsLoader assetsLoader, IJUIManager uiManager, EventManager eventManager) : base(assetsLoader, uiManager, eventManager)
//        {
//        }

//        protected override async UniTask OnEnter(object arg)
//        {
//            await base.OnEnter(arg);

//           // await tiktokSpritesManager.Initialize(); //从startupgame移过来，确保登录场景也初始化贴图管理器

//            //var controllerArgs = new UILoginController.ControllerArgs
//            //{
//            //    panelName = nameof(UIPanelLogin),
//            //};
//            //eventManager.Raise<UILoginController.Open>(controllerArgs);

//            //Debug.Log("NETWORK:" + network.GetVersion());
//        }

//        protected override string GetUISettingsName()
//        {
//            return "UISceneLoginSettings";
//        }

//        protected override ViewController[] GetControllers()
//        {
//            return controllers;
//        }

//        protected override DefaultSceneType GetSceneType()
//        {
//            return DefaultSceneType.SceneLogin;
//        }

//        protected override string GetBGMClipName()
//        {
//            return "";
//            //return tiktokConfigManager.GetLoginMusic();
//        }


//    }
//}
