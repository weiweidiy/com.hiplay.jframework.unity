using System.Collections.Generic;
using System.Linq;

namespace JFramework.Game
{
    /// <summary>
    /// 查找所有敌对单位
    /// </summary>
    public class JCombatFindAll : JCombatFinderBase
    {
        public JCombatFindAll(float[] args) : base(args) { }

        public override IJCombatExecutorExecuteArgs GetTargetsData()
        {
            var myUnitUid = GetOwner().GetCaster();
            var targetTeam = GetTargetTeam();

            if (targetTeam == null)
            {
                executeArgs.TargetUnits = new List<IJCombatCasterTargetableUnit>();
                return executeArgs;
            }

            var allUnits = targetTeam.GetAllUnits();
            if (allUnits == null)
            {
                executeArgs.TargetUnits = new List<IJCombatCasterTargetableUnit>();
                return executeArgs;
            }

            // 只保留实现 IJCombatCasterTargetableUnit 的单位，避免类型不匹配编译错误
            executeArgs.TargetUnits = allUnits.OfType<IJCombatCasterTargetableUnit>().ToList();
            return executeArgs;
        }
    }
}
