using Cysharp.Threading.Tasks;

namespace JFramework
{
    public interface ITransition
    {
        UniTask<SMTransitionState> TransitionOut();
        UniTask<SMTransitionState> TransitionIn();
    }
}
