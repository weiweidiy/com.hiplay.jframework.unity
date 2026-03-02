namespace JFramework.Unity
{
    /// <summary>
    /// 用户快速访问游戏资源的接口，提供了一个统一的接口来访问游戏资源，用户可以通过实现这个接口来快速访问游戏资源。
    /// </summary>
    public interface IGameAssetsQuary
    {
        void SetFacade(IJFacade facade);
    }
}
