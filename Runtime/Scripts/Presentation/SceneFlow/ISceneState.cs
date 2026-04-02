using Cysharp.Threading.Tasks;

namespace JFramework.Unity
{
    public interface ISceneState
    {
        string Name { get; }

        UniTask EnterAsync(ISceneContext context, object arg);

        UniTask ExitAsync();
    }
}
