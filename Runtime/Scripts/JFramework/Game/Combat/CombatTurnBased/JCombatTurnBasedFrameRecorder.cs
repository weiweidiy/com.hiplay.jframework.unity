namespace JFramework.Game
{
    public class JCombatTurnBasedFrameRecorder : IJCombatFrameRecorder
    {
        int curFrame;
        int maxFrame;
        public JCombatTurnBasedFrameRecorder(int maxFrame)
        {
            this.maxFrame = maxFrame;
        }

        /// <summary>
        /// 当前帧
        /// </summary>
        /// <returns></returns>
        public int GetCurFrame() => curFrame;

        public int GetMaxFrame() => maxFrame;

        /// <summary>
        /// 是否是最大帧
        /// </summary>
        /// <returns></returns>
        public bool IsMaxFrame()
        {
            return GetCurFrame() == GetMaxFrame();
        }

        /// <summary>
        /// 下一帧
        /// </summary>
        /// <returns></returns>
        public int NextFrame()
        {
            if (IsMaxFrame())
                return GetMaxFrame();

            curFrame += 1;
            return curFrame;
        }

        public void ResetFrame() => curFrame = 0;
    }
}
