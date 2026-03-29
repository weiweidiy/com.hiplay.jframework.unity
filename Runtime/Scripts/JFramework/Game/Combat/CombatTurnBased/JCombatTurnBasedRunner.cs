using System;
using System.Threading.Tasks;

namespace JFramework.Game
{
    /// <summary>
    /// 基于后台线程单帧运行的runner
    /// </summary>
    public class JCombatTurnBasedRunner : IJCombatTurnBasedReporter, IRunner
    {
        /// <summary>
        /// 可运行战斗对象
        /// </summary>
        protected IRunable jCombat;

        /// <summary>
        /// 战斗战报接口
        /// </summary>
        protected IJCombatTurnBasedReportBuilder jCombatReport;

        /// <summary>
        /// 事件记录器
        /// </summary>
        protected IJCombatTurnBasedEventRecorder eventRecorder;

        /// <summary>
        /// 战斗查询器
        /// </summary>
        protected IJCombatQuery jCombatQuery;

        public JCombatTurnBasedRunner(IRunable combat, IJCombatQuery jCombatQuery, IJCombatTurnBasedEventRecorder eventRecorder, IJCombatTurnBasedReportBuilder jCombatResult) { 
            this.eventRecorder = eventRecorder;
            this.jCombatQuery = jCombatQuery;
            this.jCombatReport = jCombatResult;
            this.jCombat = combat;
        }

        //public JCombatTurnBasedRunner(IRunable combat, IJCombatQuery jCombatQuery, IJCombatTurnBasedEventRecorder eventRecorder) : this(combat, jCombatQuery, eventRecorder, new JCombatTurnBasedReportBuilder()) { }

        /// <summary>
        /// 设置可执行对象
        /// </summary>
        /// <param name="combat"></param>
        public void SetRunable(IRunable combat)
        {
            jCombat = combat;
        }

        /// <summary>
        /// 获取运行结果
        /// </summary>
        /// <returns></returns>
        public IJCombatTurnBasedReportBuilder GetReport() 
        {
            return jCombatReport;
        }

        /// <summary>
        /// 运行：这里是多线程方式运行一次
        /// </summary>
        /// <returns></returns>
        public async Task Run()
        {
            IJCombatTurnBasedReportBuilder r = await Task.Run(() =>
            {
                OnBeforeStart();
                //开始战斗
                jCombat.Start(null);

                jCombat.Update(null);

                jCombat.Stop();

                OnAfterStop();

                return jCombatReport;
            });
        }

        protected virtual void OnBeforeStart()
        {
            //设置所有战斗成员数据到Report中
            jCombatReport.SetForamtionData(jCombatQuery.GetTeams());

        }

        protected virtual void OnAfterStop()
        {
            //获取胜利者
            var winner = jCombatQuery.GetWinner();

            //设置结果
            jCombatReport.SetCombatEvents(eventRecorder.GetAllCombatEvents());
            jCombatReport.SetCombatWinner(winner);
            
        }


    }
}
