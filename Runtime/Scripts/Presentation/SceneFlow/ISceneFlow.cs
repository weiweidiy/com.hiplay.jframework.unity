using Cysharp.Threading.Tasks;

namespace JFramework.Unity
{
    public interface ISceneFlow
    {
        UniTask EnterFirstAsync();

        UniTask EnterAsync<TState>(object arg = null) where TState : class, ISceneState;
    }
}
