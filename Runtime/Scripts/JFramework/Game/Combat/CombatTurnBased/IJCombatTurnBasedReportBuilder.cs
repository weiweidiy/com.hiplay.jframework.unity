using System.Collections.Generic;

namespace JFramework.Game
{
    /// <summary>
    /// 战斗结果，player就是读取这个对象
    /// </summary>
    public interface IJCombatTurnBasedReportBuilder
    {
        void SetForamtionData(List<IJCombatTeam> teams);

        /// <summary>
        /// 设置所有战斗事件
        /// </summary>
        /// <param name="events"></param>
        void SetCombatEvents(List<JCombatTurnBasedEvent> events);

        /// <summary>
        /// 设置胜利队伍
        /// </summary>
        /// <param name="team"></param>
        void SetCombatWinner(IJCombatTeam team);

        /// <summary>
        /// 获取战报
        /// </summary>
        /// <returns></returns>
        JCombatTurnBasedReportData<T> GetCombatReportData<T>() where T :class, IJCombatUnitData;
    }
}
