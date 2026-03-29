namespace JFramework
{
    /// <summary>
    /// 可设置席位接口
    /// </summary>
    public interface ILocatable<T>
    {
        /// <summary>
        /// 获取成员队伍位置
        /// </summary>
        /// <returns></returns>
        int GetMemberLocationInTeam(T unit);

        /// <summary>
        /// 设置成员在队伍中的位置
        /// </summary>
        /// <param name="location"></param>
        void SetMemberLocationInTeam(T unit, int location);

    }
         
}

