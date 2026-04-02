using Cysharp.Threading.Tasks;

namespace JFramework.Unity
{
    public interface IAsyncDisposableModule
    {
        UniTask DisposeAsync();
    }
}
