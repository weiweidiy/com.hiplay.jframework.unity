
using Cysharp.Threading.Tasks;


namespace JFramework.Unity
{

    /// <summary>
    /// SMTransition提供者
    /// </summary>
    public class SMTransitionProvider : ITransitionProvider
    {

        IAssetsLoader assetLoader;

        public SMTransitionProvider(IAssetsLoader assetLoader)
        {
            this.assetLoader = assetLoader;
        }

        public async UniTask<ITransition> InstantiateAsync(string transitionName)
        {
            // to do : 增加一个location manager用于定位资源位置
            var go = await assetLoader.InstantiateAsync(transitionName);

            var result = go.GetComponent<ITransition>();

            return result;
        }
    }
}
