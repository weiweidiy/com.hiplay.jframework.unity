using Cysharp.Threading.Tasks;

namespace JFramework.Unity
{
    public interface IJApp
    {
        T GetService<T>() where T : class;

        UniTask RunAsync();

        UniTask ShutdownAsync();
    }
}
