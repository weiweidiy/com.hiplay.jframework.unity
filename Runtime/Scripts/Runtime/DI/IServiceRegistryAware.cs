namespace JFramework.Unity
{
    /// <summary>
    /// 注册服务中间件
    /// </summary>
    public interface IServiceRegistryAware
    {
        IServiceRegistry Services { get; set; }
    }
}
