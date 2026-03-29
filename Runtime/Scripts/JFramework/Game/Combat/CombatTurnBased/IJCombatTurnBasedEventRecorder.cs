using System.Collections.Generic;

namespace JFramework.Game
{
    /// <summary>
    /// 战斗事件记录器
    /// </summary>
    public interface IJCombatTurnBasedEventRecorder
    {
        List<JCombatTurnBasedEvent> GetAllCombatEvents();

        /// <summary>
        /// 创建一个新的战斗事件
        /// </summary>
        /// <returns></returns>
        JCombatTurnBasedEvent CreateActionEvent(string casterUid, int curHp, int maxHp);

        JCombatTurnBasedEvent GetCurrentActionEvent();

        /// <summary>
        /// 添加一个战斗事件
        /// </summary>
        /// <param name="combatEvent"></param>
        void AddEvent(JCombatTurnBasedEvent combatEvent);
    }
}
