using System;
using System.Collections.Generic;
using System.Linq;

namespace JFramework.Game
{
    public class JCombatSeatFuncBuilder : IJCombatSeatDelegateBuilder
    {
        List<JCombatFormationInfo> formations;
        public JCombatSeatFuncBuilder(List<JCombatFormationInfo> formations) => this.formations = formations;

        public Func<string, int> Build()
        {
            return (unitUid) =>
            {
                //从atkFormations中获取对应的阵型点位
                if (formations == null || formations.Count == 0)
                {
                    throw new Exception("没有可用的阵型");
                }
                var formation = formations.FirstOrDefault(f => f.UnitInfo.Uid == unitUid);
                return formation?.Point ?? -1; // 如果没有找到对应的阵型点位，返回-1
            };
        }
    }
}
