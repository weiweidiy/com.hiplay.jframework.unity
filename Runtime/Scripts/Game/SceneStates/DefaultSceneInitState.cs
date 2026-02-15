
//using Cysharp.Threading.Tasks;
//using Game.Common;
//using JFramework;

//namespace JFramework.Unity
//{
//    public class DefaultSceneInitState : DefaultBaseSceneState
//    {
//        public DefaultSceneInitState(IAssetsLoader assetsLoader, IJUIManager uiManager, EventManager eventManager) : base(assetsLoader, uiManager, eventManager)
//        {
//        }

//        /// <summary>
//        /// 必须重写，因为默认状态不需要切换场景，所以不调用SwitchScene方法，直接完成即可
//        /// </summary>
//        /// <param name="arg"></param>
//        /// <returns></returns>
//        protected override UniTask OnEnter(object arg)
//        {
//            return UniTask.CompletedTask;
//        }
//        public override UniTask OnExit()
//        {
//            return UniTask.CompletedTask;
//        }

//        protected override string GetBGMClipName()
//        {
//            throw new System.NotImplementedException();
//        }

//        //protected override ViewController[] GetControllers()
//        //{
//        //    throw new System.NotImplementedException();
//        //}

//        protected override DefaultSceneType GetSceneType()
//        {
//            return DefaultSceneType.Init;
//        }

//        //protected override IAssetsLoader assetsLoader => throw new System.NotImplementedException();
//        protected override string GetUISettingsName()
//        {
//            throw new System.NotImplementedException();
//        }

//        protected override ViewController[] GetControllers()
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}
