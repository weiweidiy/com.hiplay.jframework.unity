using System.Collections.Generic;

namespace JFramework.Game
{
    /// <summary>
    /// 回合开始触发器
    /// </summary>
    public class JCombatTriggerTurnStart : JCombatTriggerBase
    {
        int count = 0;
        public JCombatTriggerTurnStart(float[] args, IJCombatTargetsFinder finder) : base(args, finder)
        {

        }

        protected override int GetValidArgsCount()
        {
            return 3;
        }

        int GetTurnMode()
        {
            return (int)GetArg(0);
        }

        int GetTargetTurn()
        {
            return (int)GetArg(1);
        }

        int GetMaxCount()
        {
            return (int)GetArg(2);
        }

        protected override void OnStart(RunableExtraData extraData)
        {
            base.OnStart(extraData);

            var combat = query.GetCombat() as JCombatTurnBased;
            combat.onCombatTurnStart += Combat_onCombatTurnStart;
        }

        protected override void OnStop()
        {
            base.OnStop();
            var combat = query.GetCombat() as JCombatTurnBased;
            combat.onCombatTurnStart -= Combat_onCombatTurnStart;
        }

        private void Combat_onCombatTurnStart(int frame)
        {
            if(count >= GetMaxCount())
                return;

            var needTrigger = false;
            if (GetTurnMode() == 0) // 0表示每回合都触发
                needTrigger = true;


            if (GetTurnMode() == 1 && frame == GetTargetTurn()) // 1表示在指定回合触发
                needTrigger = true;


            if (GetTurnMode() == 2 && frame % 2 == GetTargetTurn()) //GetTargetTurn为1时，奇数回合触发，为0时偶数回合触发
                needTrigger = true;

            if (!needTrigger)
                return;

            if (finder != null)
                TriggerOn(finder.GetTargetsData());
            else
                TriggerOn(null);

            count++;
        }
    }
}
