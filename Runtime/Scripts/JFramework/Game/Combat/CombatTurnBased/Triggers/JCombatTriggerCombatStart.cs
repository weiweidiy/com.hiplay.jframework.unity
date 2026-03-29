using System;

namespace JFramework.Game
{
    /// <summary>
    /// 战斗开始时，触发一次
    /// </summary>
    public class JCombatTriggerCombatStart : JCombatTriggerBase
    {
        public JCombatTriggerCombatStart(float[] args, IJCombatTargetsFinder finder) : base(args, finder)
        {
        }

        protected override int GetValidArgsCount()
        {
            return 0;
        }

        protected override void OnStart(RunableExtraData extraData)
        {
            base.OnStart(extraData);

            var combat = query.GetCombat() as JCombatTurnBased;
            combat.onCombatStart += TriggerOnCombatStart;
        }

        protected override void OnStop()
        {
            base.OnStop();
            var combat = query.GetCombat() as JCombatTurnBased;
            combat.onCombatStart -= TriggerOnCombatStart;
        }

        private void TriggerOnCombatStart()
        {
            if (finder != null)
                TriggerOn(finder.GetTargetsData()); // 战斗开始时触发一次
            else
                TriggerOn(null);
        }


    }


}
