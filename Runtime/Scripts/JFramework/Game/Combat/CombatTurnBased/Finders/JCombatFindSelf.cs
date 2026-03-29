using System.Collections.Generic;

namespace JFramework.Game
{
    /// <summary>
    /// 找自己
    /// </summary>
    public class JCombatFindSelf : JCombatFinderBase
    {
        public JCombatFindSelf(float[] args) : base(args)
        {
        }

        public override IJCombatExecutorExecuteArgs GetTargetsData()
        {
            executeArgs.TargetUnits = GetTargets();
            return executeArgs;
        }

        protected virtual List<IJCombatCasterTargetableUnit> GetTargets()
        {
            var result = new List<IJCombatCasterTargetableUnit>();

            var primaryTarget = FindPrimaryTarget();

            if (primaryTarget != null)
            {
                result.Add(primaryTarget);
            }
            else
            {
                // 如果没有找到主目标，则返回空列表
                return result;
            }

            return result;
        }

        /// <summary>
        /// 查找主目标
        /// </summary>
        /// <returns></returns>
        IJCombatCasterTargetableUnit FindPrimaryTarget()
        {
            var myUnitUid = GetOwner().GetCaster();           
            return query.GetUnit(myUnitUid) as IJCombatCasterTargetableUnit;
        }


    }
}
