using System.Collections.Generic;

namespace JFramework.Game
{
    /// <summary>
    /// 执行器执行参数
    /// </summary>
    public class ExecutorExecuteArgs : IJCombatExecutorExecuteArgs
    {
        public IJCombatDamageData DamageData { get; set; }
        public List<IJCombatCasterTargetableUnit> TargetUnits { get ; set; }
        public Dictionary<string, IJCobmatExecuteArgsHistroy> ExecuteArgsHistroy { get ; set; }
        public IJCombatCasterUnit CasterUnit { get ; set ; }

        public void Clear()
        {
            DamageData = null;
            TargetUnits = null;
            ExecuteArgsHistroy?.Clear();
            ExecuteArgsHistroy = null;
        }
    }
}
