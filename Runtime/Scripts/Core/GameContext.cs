namespace JFramework.Unity
{
    /// <summary>
    /// 默认的场景状态机上下文类。
    /// </summary>
    public class GameContext : IServiceRegistryAware
    {
        public IServiceRegistry Services { get; set; }
    }
}
