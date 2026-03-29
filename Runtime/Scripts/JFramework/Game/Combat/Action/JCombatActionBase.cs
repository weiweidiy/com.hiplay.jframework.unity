using System;
using System.Collections.Generic;

namespace JFramework.Game
{

    /// <summary>
    /// 战斗行为基类，触发器触发执行，
    /// </summary>
    public class JCombatActionBase : BaseRunable, IJCombatAction
    {
        public string Uid { get; private set; }

        IJCombatQuery query;

        List<IJCombatTrigger> triggers;
        IJCombatTargetsFinder finder;
        List<IJCombatExecutor> executors;

        IJCombatCaster caster;

        IJCombatTurnBasedEventRecorder eventRecorder;

        IJCombatContext context;

        IJCombatAcionInfo actionInfo;

        public JCombatActionBase(IJCombatAcionInfo actionInfo, IJCombatContext context) : this(actionInfo.Uid, actionInfo.Triggers, actionInfo.Finder, actionInfo.Executors, context.EventRecorder)
        {
            this.actionInfo = actionInfo;
            this.context = context;
        }

        public JCombatActionBase(string uid, List<IJCombatTrigger> triggers, IJCombatTargetsFinder finder, List<IJCombatExecutor> executors, IJCombatTurnBasedEventRecorder eventRecorder)
        {
            this.Uid = uid;
            this.triggers = triggers;
            this.executors = executors;
            this.eventRecorder = eventRecorder;
            this.finder = finder;

            if (triggers != null)
            {
                foreach (IJCombatTrigger trigger in triggers)
                {
                    //设置父节点
                    trigger.SetOwner(this);
                }
            }

            if (finder != null)
            {
                //设置父节点
                finder.SetOwner(this);
            }

            if (executors != null)
            {
                foreach (IJCombatExecutor executor in executors)
                {
                    //设置父节点
                    executor.SetOwner(this);
                }
            }
        }

        public void SetQuery(IJCombatQuery query)
        {
            this.query = query;

            if (triggers != null)
            {
                foreach (IJCombatTrigger trigger in triggers)
                {

                    trigger.SetQuery(query);
                }
            }

            if (finder != null)
            {
                finder.SetQuery(query);
            }

            if (executors != null)
            {
                foreach (IJCombatExecutor executor in executors)
                {

                    executor.SetQuery(query);
                }
            }
        }

        protected override void OnStart(RunableExtraData extraData)
        {
            base.OnStart(extraData);

            if (triggers != null)
            {
                foreach (var trigger in triggers)
                {
                    trigger.Start(extraData);
                    trigger.onTriggerOn += Trigger_onTriggerOn;
                }
            }

            if (finder != null)
            {
                finder.Start(extraData);
            }

            if (executors != null)
            {
                foreach (var executor in executors)
                {
                    executor.Start(extraData);
                }
            }
        }

        protected override void OnStop()
        {
            base.OnStop();

            if (triggers != null)
            {
                foreach (IJCombatTrigger trigger in triggers)
                {
                    trigger.Stop();
                    trigger.onTriggerOn -= Trigger_onTriggerOn;
                }
            }

            if (finder != null)
            {
                finder.Stop();
            }

            if (executors != null)
            {
                foreach (var executor in executors)
                {
                    executor.Stop();
                }
            }
        }

        private void Trigger_onTriggerOn(IJCombatTrigger trigger, IJCombatExecutorExecuteArgs executeArgs)
        {
            Execute(executeArgs);
            trigger.Reset(); // 重置触发器状态
        }


        public void Execute(IJCombatExecutorExecuteArgs executeArgs = null)
        {
            if (executors != null)
            {
                if (finder != null)
                {
                    //使用查找器找到的目标
                    executeArgs = finder.GetTargetsData();
                }      

                if(executeArgs == null)
                {
                    return;
                }

                //创建一个空的执行日志对象，用来记录执行日志
                //var newActionEvent = eventRecorder.CreateActionEvent(GetCaster(), Uid);
                foreach (var executor in executors)
                {
                    if(context != null && context.Logger != null)
                    {
                        // 记录执行日志
                        //context.Logger.Log($"{caster.Uid} Executing action {Uid} with executor {executor.GetType().Name}");
                    }
                    //executor.AddCombatEvent(newActionEvent);
                    executeArgs = executor.Execute(executeArgs);
                }

                //if (targets != null)
                //{
                //    foreach (var target in targets)
                //    {
                //        IJCombatExecutorExecuteArgs args = null;
                //        foreach (var executor in executors)
                //        {
                //            args = executor.Execute(executeArgs, args, target);
                //        }
                //    }
                //}
                //else
                //{
                //    IJCombatExecutorExecuteArgs args = null;
                //    foreach (var executor in executors)
                //    {
                //        args = executor.Execute(executeArgs, args, null);
                //    }
                //}


            }
        }

        /// <summary>
        /// 设置actin释放者查询器
        /// </summary>
        /// <param name="casterQuery"></param>
        public void SetCaster(IJCombatCaster casterQuery) => this.caster = casterQuery;

        /// <summary>
        /// 获取action释放者uid
        /// </summary>
        /// <returns></returns>
        public string GetCaster() => caster.Uid;

        public bool CanCast()
        {
            // 这里可以添加一些逻辑来判断是否可以施放，比如检查是否有足够的资源，或者是否满足某些条件
            return true;

            //throw new NotImplementedException();
        }

        /// <summary>
        /// 直接释放，不需要触发器触发
        /// </summary>
        public void Cast()
        {
            Execute();
        }

        /// <summary>
        /// 获取所有触发器
        /// </summary>
        /// <returns></returns>
        public List<IJCombatTrigger> GetTriggers()
        {
            return triggers;
        }

        public IJCombatAcionInfo GetActionInfo()
        {
            return actionInfo;
        }
    }
}
