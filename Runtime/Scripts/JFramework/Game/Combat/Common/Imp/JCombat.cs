using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JFramework.Game
{
    /// <summary>
    /// 可以战斗播报化的对象
    /// </summary>
    public abstract class JCombat : BaseRunable
    {
        /// <summary>
        /// 可运行列表，这里应该是combatTeam
        /// </summary>
        List<IRunable> runables; 

        public JCombat(List<IRunable> runables)
        {
            this.runables = runables;
        }

        /// <summary>
        /// 战斗开始
        /// </summary>
        protected override void OnStart(RunableExtraData extraData)
        {
            base.OnStart(extraData);

            foreach(var runable in runables)
            {
                runable.Start(null);
            }
        }

        /// <summary>
        /// 逻辑更新
        /// </summary>
        protected override abstract void OnUpdate(RunableExtraData extraData);

        /// <summary>
        /// 战斗结束
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();

            foreach (var runable in runables)
            {
                runable.Stop();
            }

        }


    }
}
