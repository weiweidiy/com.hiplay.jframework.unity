using System.Collections.Generic;

namespace JFramework.Game
{
    /// <summary>
    /// 命中之前触发，可以改变伤害数据，带有回合数限制
    /// </summary>
    public class JCombatBeforeHittingExtraWithTurnTrigger : JCombatBeforeHittingTrigger
    {
        public JCombatBeforeHittingExtraWithTurnTrigger(float[] args, IJCombatTargetsFinder finder) : base(args, finder)
        {
        }
        protected override int GetValidArgsCount()
        {
            return base.GetValidArgsCount() + 1; // 多一个参数
        }

        int GetTurnCount()
        {
            return (int)GetArg(0);
        }

        protected override void Target_onBeforeHitting(IJCombatCasterUnit caster, IJCombatDamageData data, IJCombatTargetable target)
        {
            var curTurn = query.GetCurFrame();
            if (curTurn != GetTurnCount())
                return;

            executeArgs.DamageData = data;
            executeArgs.TargetUnits = new List<IJCombatCasterTargetableUnit> { target as IJCombatCasterTargetableUnit };
            TriggerOn(executeArgs);
        }
    }


}
