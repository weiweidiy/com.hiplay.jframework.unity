using System.Collections.Generic;

namespace JFramework.Game
{
    public class JCombatFormationInfo
    {
        public int Point { get; set; }
        public IJCombatUnitInfo UnitInfo { get; set; }
    }

    public interface IJCombatUnitInfo
    {
        // 定义通用的单位信息接口
        string Uid { get; set; }

        List<IUnique> AttrList { get; set; }

        List<IJCombatAction> Actions { get; set; }
    }
}
