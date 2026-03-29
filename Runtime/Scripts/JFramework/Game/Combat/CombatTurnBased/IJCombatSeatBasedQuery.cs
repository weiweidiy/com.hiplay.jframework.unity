using System.Collections.Generic;

namespace JFramework.Game
{
    /// <summary>
    /// 基于站位的查询接口
    /// </summary>
    public interface IJCombatSeatBasedQuery
    {
        int GetSeat(string unitUid);

        ///// <summary>
        ///// 根据座位列表，返回存在的unit列表
        ///// </summary>
        ///// <param name="seats"></param>
        ///// <returns></returns>
        //List<IJCombatCasterTargetableUnit> GetUnits(IJCombatTeam team, List<int> seats);

        IJCombatCasterTargetableUnit GetUnit(IJCombatTeam team, int seat);
    }
}
