namespace JFramework.Game
{
    /// <summary>
    /// 战斗更新逻辑次数统计
    /// </summary>
    public interface IJCombatFrameRecorder
    {
        /// <summary>
        /// 重置帧数
        /// </summary>
        void ResetFrame();
        /// <summary>
        /// 获取当前帧
        /// </summary>
        /// <returns></returns>
        int GetCurFrame();

        /// <summary>
        /// 获取最大帧
        /// </summary>
        /// <returns></returns>
        int GetMaxFrame();
        /// <summary>
        /// 下一帧
        /// </summary>
        /// <returns></returns>
        int NextFrame();

        /// <summary>
        /// 是否达到最大帧
        /// </summary>
        /// <returns></returns>
        bool IsMaxFrame();
    }
}
