using Cysharp.Threading.Tasks;


namespace JFramework.Unity
{
    public interface ITransitionProvider
    {
        UniTask<ITransition>  InstantiateAsync(string transitionType);
    }
}
