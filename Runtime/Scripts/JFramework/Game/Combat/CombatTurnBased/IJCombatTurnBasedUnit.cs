using System.Collections.Generic;

namespace JFramework.Game
{
    public interface IJCombatTurnBasedUnit : IJCombatCasterTargetableUnit, IJCombatCaster
    {
        /// <summary>
        /// 获取行动点，用于排序
        /// </summary>
        /// <returns></returns>
        int GetActionPoint();

        IJCombatUnitInfo GetUnitInfo();

        List<IJCombatAcionInfo> GetActionInfos();
    }
}
