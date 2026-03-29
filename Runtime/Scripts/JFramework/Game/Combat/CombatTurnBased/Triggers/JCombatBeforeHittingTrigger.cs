using System.Collections.Generic;

namespace JFramework.Game
{
    /// <summary>
    /// 单位命中之前触发，可以改变伤害数据
    /// </summary>
    public class JCombatBeforeHittingTrigger : JCombatTriggerBase
    {
        List<IJCombatCasterTargetableUnit> targets;
        public JCombatBeforeHittingTrigger(float[] args, IJCombatTargetsFinder finder) : base(args, finder) { }

        protected override int GetValidArgsCount()
        {
            return 0; // 不需要参数
        }

        protected override void OnStart(RunableExtraData extraData)
        {
            base.OnStart(extraData);
            if(finder == null)
            {
                throw new System.Exception("JCombatBeforeHittingTrigger requires a finder to be set.");
            }
            var executeArgs = finder.GetTargetsData();
            targets = executeArgs.TargetUnits;
            foreach (var target in targets)
            {
                target.onBeforeHitting += Target_onBeforeHitting;
            }
        }

        protected override void OnStop()
        {
            base.OnStop();

            if (targets == null)
                return;
            foreach (var targetable in targets)
            {
                if (targetable != null)
                {
                    targetable.onBeforeHitting -= Target_onBeforeHitting;
                }
            }
            targets.Clear();
        }

        protected virtual void Target_onBeforeHitting(IJCombatCasterUnit caster, IJCombatDamageData data, IJCombatTargetable target)
        {
            //executeArgs.Clear();
            executeArgs.DamageData = data;
            executeArgs.TargetUnits = new List<IJCombatCasterTargetableUnit> { target as IJCombatCasterTargetableUnit };
            TriggerOn(executeArgs);
        }


    }


}
