using JFramework;
using System;
using System.Collections.Generic;

namespace JFramework.Game
{
    /// <summary>
    /// 战斗行动的选择器 
    /// </summary>
    public interface IJCombatTurnBasedCasterSelector
    {
        /// <summary>
        /// 获取出手序列（已排序）
        /// </summary>
        /// <returns></returns>
        List<IJCombatTurnBasedUnit> GetActionUnits();
        /// <summary>
        /// 获取当前可行动的单位
        /// </summary>
        /// <returns></returns>
        IJCombatTurnBasedUnit PopActionUnit();

        /// <summary>
        /// 重置actionList
        /// </summary>
        void ResetActionUnits();

        /// <summary>
        /// 设置列表（未排序）
        /// </summary>
        /// <param name="units"></param>
        void AddUnits(List<IJCombatTurnBasedUnit> units);

        /// <summary>
        /// 是否全部完成了
        /// </summary>
        /// <returns></returns>
        bool IsAllComplete();
    }


}
