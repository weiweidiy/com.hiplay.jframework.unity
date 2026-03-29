using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JFramework.Game
{

    /// <summary>
    /// 伤害执行器
    /// </summary>
    public class JCombatExecutorDamage : JCombatExecutorBase
    {


        public JCombatExecutorDamage(IJCombatContext context, IJCombatFilter filter, IJCombatTargetsFinder finder, IJCombatFormula formulua, float[] args)
            : base(context, filter, finder, formulua, args)
        {
        }
        protected override int GetValidArgsCount()
        {
            return 0; // 只需要一个参数，通常是伤害值或相关系数
        }

        protected override IJCobmatExecuteArgsHistroy DoExecute(IJCombatExecutorExecuteArgs executeArgs, IJCombatCasterTargetableUnit target)
        {
            //优先使用查找器找到的目标
            if (target != null)
            {
                return DoDamage(target, executeArgs);
            }
            throw new Exception("JCombatExecutorDamage: No targets found for damage execution.");
        }

        IJCobmatExecuteArgsHistroy DoDamage(IJCombatCasterTargetableUnit target, IJCombatExecutorExecuteArgs executeArgs)
        {
            var executeArgsHistroy = new JCombatExecutorExecuteArgsHistroy();

            var uid = Guid.NewGuid().ToString();

            float hitValue = 0;
            formulua.CalcHitValue(target, ref hitValue, executeArgs);
            hitValue = Math.Max(1, hitValue); // 确保伤害值不为负

            var sourceUnitUid = GetOwner().GetCaster();
            var sourceActionUid = GetOwner().Uid;
            var data = new JCombatDamageData(uid, sourceUnitUid, sourceActionUid, (int)hitValue, 0, target.Uid);
            //设置执行历史记录
            executeArgsHistroy.DamageData = data;

            var sourceUnit = query.GetUnit(sourceUnitUid);
            var caster = sourceUnit as IJCombatCasterUnit;
            caster.NotifyBeforeHitting(data,target);
            target.NotifyBeforeHurt(data, caster, executeArgs);

            // 受伤
            var minusHp = target.OnHurt(data);

            var logger = query.GetLogger();
            if (logger != null)
            {
                logger.Log($"Frame: {query.GetCurFrame()}, Damage: {data.GetDamage()} from {sourceUnitUid} to {target.Uid}" +
                    $", hitValue: {hitValue}, minusHp: {minusHp} , targetHp: {target.GetCurHp()}");
            }

            caster.NotifyAfterHitted(data);
            target.NotifyAfterHurt(data, caster, executeArgs);

            var casterTargetUnit = caster as IJCombatCasterTargetableUnit;

            var objEvent = context?.EventRecorder.GetCurrentActionEvent();
            if (objEvent != null)
            {
                var actionEvent = objEvent.ActionEvents.Where(e => e.ActionUid == GetOwner().Uid).SingleOrDefault();
                if(actionEvent == null)
                {
                    actionEvent = new ActionEvent();
                    actionEvent.CasterUid = casterTargetUnit.Uid;
                    actionEvent.ActionUid = GetOwner().Uid;
                    objEvent.ActionEvents.Add(actionEvent);
                }
                
                if(!actionEvent.ActionEffect.ContainsKey(CombatEventActionType.Damage))
                    actionEvent.ActionEffect.Add(CombatEventActionType.Damage, new List<ActionEffectInfo>());

                var actionEffectList = actionEvent.ActionEffect[CombatEventActionType.Damage];
                actionEffectList.Add(new ActionEffectInfo()
                {
                    TargetUid = target.Uid,
                    Value = data.GetDamage(),
                    TargetHp = target.GetCurHp(),
                    TargetMaxHp = target.GetMaxHp(),
                    CasterHp = casterTargetUnit.GetCurHp(),
                    CasterMaxHp = casterTargetUnit.GetMaxHp()
                });
            }

            return executeArgsHistroy;
        }
    }
}
