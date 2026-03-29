namespace JFramework
{
    /// <summary>
    /// 存活检查
    /// </summary>
    public interface IAliveChecker<T>
    {
        /// <summary>
        /// 是否还有存活单位
        /// </summary>
        /// <returns></returns>
        bool HasAliveUnit();

        /// <summary>
        /// 获取存活数量
        /// </summary>
        /// <returns></returns>
        int GetAliveUnitCount();

        /// <summary>
        /// 获取下一个存活的成员
        /// </summary>
        /// <returns></returns>
        T GetNextAliveUnit();

    }
}

