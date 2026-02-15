
using Cysharp.Threading.Tasks;
using Game.Common;
using JFramework;

namespace JFramework.Unity
{
    public class DefaultSceneInitState : DefaultBaseSceneState
    {
        public DefaultSceneInitState(IAssetsLoader assetsLoader, IJUIManager uiManager, EventManager eventManager) : base(assetsLoader, uiManager, eventManager)
        {
        }

        protected override UniTask OnEnter(object arg)
        {
            return UniTask.CompletedTask;
        }
        public override UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }

        protected override string GetBGMClipName()
        {
            throw new System.NotImplementedException();
        }

        //protected override ViewController[] GetControllers()
        //{
        //    throw new System.NotImplementedException();
        //}

        protected override DefaultSceneType GetSceneType()
        {
            return DefaultSceneType.Init;
        }

        //protected override IAssetsLoader AssetsLoader => throw new System.NotImplementedException();
        protected override string GetUISettingsName()
        {
            throw new System.NotImplementedException();
        }

        protected override ViewController[] GetControllers()
        {
            throw new System.NotImplementedException();
        }
    }
}
