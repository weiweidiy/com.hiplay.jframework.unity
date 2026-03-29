namespace JFramework
{
    /// <summary>
    /// 获取更新地址接口
    /// </summary>
    public interface IRouter
    {
        /// <summary>
        /// 获取更新地址
        /// </summary>
        /// <returns></returns>
        RouteResponse GetHotFixAddress(RouteRequest routeInfo);
    }

    /// <summary>
    /// 路由请求
    /// </summary>
    public struct RouteRequest
    {
        public string routeUrl;
        //string appVersion;
        //string platform;
    }

    /// <summary>
    /// 路由答复
    /// </summary>
    public struct RouteResponse
    {
        /// <summary>
        /// 更新资源地址
        /// </summary>
        public string resAddress;
    }
}

