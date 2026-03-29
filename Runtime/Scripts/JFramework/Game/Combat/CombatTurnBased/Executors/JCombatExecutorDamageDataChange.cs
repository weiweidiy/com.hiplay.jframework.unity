using System;
using System.Collections.Generic;
using System.Linq;

namespace JFramework.Game
{
    /// <summary>
    /// 伤害提升执行器，一定是伤害执行前，比如JCombatBeforeHurtTrigger触发
    /// </summary>
    public class JCombatExecutorDamageDataChange : JCombatExecutorBase
    {

        public JCombatExecutorDamageDataChange(IJCombatContext context, IJCombatFilter filter, IJCombatTargetsFinder finder, IJCombatFormula formulua, float[] args) : base(context,filter, finder, formulua, args)
        {
        }

        protected override int GetValidArgsCount()
        {
            return 0; // 只需要一个参数，通常是伤害值或相关系数
        }


        protected override IJCobmatExecuteArgsHistroy DoExecute(IJCombatExecutorExecuteArgs executeArgs, IJCombatCasterTargetableUnit target)
        {
            var damageData = executeArgs.DamageData;
            if (damageData == null)
            {
                return new JCombatExecutorExecuteArgsHistroy();
            }

            var value = (float)damageData.GetDamage();
            formulua.CalcHitValue(null, ref value, executeArgs); // 假设伤害提升20%
            value = Math.Max(1, value); // 确保伤害值不为负v0
            damageData.SetDamage((int)value);


            var objEvent = context?.EventRecorder.GetCurrentActionEvent();
            if (objEvent != null)
            {
                var actionEvent = objEvent.ActionEvents.Where(e => e.ActionUid == GetOwner().Uid).SingleOrDefault();
                if (actionEvent == null)
                {
                    actionEvent = new ActionEvent();
                    actionEvent.CasterUid = GetOwner().GetCaster();
                    actionEvent.ActionUid = GetOwner().Uid;
                    objEvent.ActionEvents.Add(actionEvent);
                }
            }


            return new JCombatExecutorExecuteArgsHistroy() { DamageData = damageData};
        }
    }
}
