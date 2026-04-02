using JFramework;

namespace JFramework.Unity
{
    public interface IViewContext
    {
        IJUIManager UI { get; }

        IGameObjectManager Objects { get; }

        EventManager Events { get; }
    }
}
