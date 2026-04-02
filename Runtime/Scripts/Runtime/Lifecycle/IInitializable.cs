using Cysharp.Threading.Tasks;

namespace JFramework.Unity
{
    public interface IInitializable
    {
        UniTask InitializeAsync();
    }
}
