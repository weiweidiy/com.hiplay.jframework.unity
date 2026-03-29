using System.Collections.Generic;
using System.Reflection;

namespace JFramework.Game
{
    /// <summary>
    /// 复合执行器，执行一系列的战斗动作
    /// </summary>
    public class JCombatMacroExecutor : JCombatActionComponent, IJCombatExecutor
    {
        List<JCombatExecutorBase> executors;
        public JCombatMacroExecutor(List<JCombatExecutorBase> executors , float[] args) : base(args)
        {
            this.executors = executors ?? new List<JCombatExecutorBase>();
        }

        protected override int GetValidArgsCount()
        {
            return 0; // 不需要参数
        }

        //public void AddCombatEvent(JCombatTurnBasedEvent combatEvent)
        //{
        //    foreach (var executor in executors)
        //    {
        //        executor.AddCombatEvent(combatEvent);
        //    }
        //}

        public IJCombatExecutorExecuteArgs Execute(IJCombatExecutorExecuteArgs executorArgs)
        {
            foreach (var executor in executors)
            {
                executorArgs = executor.Execute(executorArgs);
            }
            return executorArgs;
        }

        public override void SetOwner(IJCombatAction owner)
        {
            base.SetOwner(owner);
            foreach (var executor in executors)
            {
                executor.SetOwner(owner);
            }
        }

        public override void SetQuery(IJCombatQuery query)
        {
            base.SetQuery(query);
            foreach (var executor in executors)
            {
                executor.SetQuery(query);
            }

        }

        protected override void OnStart(RunableExtraData extraData)
        {
            base.OnStart(extraData);
            foreach (var executor in executors)
            {
                executor.Start(extraData);
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
            foreach (var executor in executors)
            {
                executor.Stop();
            }
        }

        protected override void OnUpdate(RunableExtraData extraData)
        {
            base.OnUpdate(extraData);
            foreach (var executor in executors)
            {
                executor.Update(extraData);
            }
        }
    }
}
