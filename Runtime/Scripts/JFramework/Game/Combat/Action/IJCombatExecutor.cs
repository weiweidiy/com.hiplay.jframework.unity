using JFramework;
using System;
using System.Collections.Generic;

namespace JFramework.Game
{
    public interface IJCombatExecutor : IJCombatActionComponent /*IJCombatLifeCycle*/
    {
        //void AddCombatEvent(JCombatTurnBasedEvent combatEvent);

        /// <summary>
        /// 执行战斗行为
        /// </summary>
        /// <param name="triggerArgs">触发器传入的参数，不同触发器传入的类型不同</param>
        /// <param name="executorArgs">上一个执行器处理的结果参数，可能为空，不同的执行器传入的参数不同</param>
        /// <param name="target">执行的目标</param>
        IJCombatExecutorExecuteArgs Execute(IJCombatExecutorExecuteArgs executorArgs);
 
    }
}
