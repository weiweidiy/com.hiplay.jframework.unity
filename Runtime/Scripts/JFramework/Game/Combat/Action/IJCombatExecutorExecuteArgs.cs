using System.Collections.Generic;

namespace JFramework.Game
{
    /// <summary>
    /// 执行器执行参数
    /// </summary>
    public interface IJCombatExecutorExecuteArgs
    {
        /// <summary>
        /// 释放者
        /// </summary>
        IJCombatCasterUnit CasterUnit { get; set; }

        /// <summary>
        /// 执行的目标对象列表，必须要有
        /// </summary>
        List<IJCombatCasterTargetableUnit> TargetUnits { get; set; }

        /// <summary>
        /// 对伤害处理的执行器需要这个参数
        /// </summary>
        IJCombatDamageData DamageData { get; set; }

        /// <summary>
        /// 执行参数历史
        /// </summary>
        Dictionary<string, IJCobmatExecuteArgsHistroy> ExecuteArgsHistroy { get; set; }

        void Clear();
    }

    /// <summary>
    /// 执行器执行参数历史
    /// </summary>
    public interface IJCobmatExecuteArgsHistroy
    {

    }

    /// <summary>
    /// 执行器执行参数历史-伤害
    /// </summary>
    public class JCombatExecutorExecuteArgsHistroy : IJCobmatExecuteArgsHistroy
    {
        public IJCombatDamageData DamageData { get; set; }
    }

}
