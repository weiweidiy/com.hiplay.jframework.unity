using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace JFramework.Game
{
    public abstract class JCombatBasePlayer<T> : BaseRunable, IJCombatTurnBasedPlayer<T> where T : IJCombatUnitData
    {
        protected JCombatTurnBasedReportData<T> reportData;

        float scale = 1f;

        protected IObjectPool pool;

        protected IJCombatAnimationPlayer animationPlayer;

        protected ILogger loger;

        public JCombatBasePlayer( JCombatTurnBasedReportData<T> reportData, IJCombatAnimationPlayer animationPlayer, IObjectPool objPool = null, ILogger loger = null)
        {
            this.pool = objPool;
            this.reportData = reportData;
            this.animationPlayer = animationPlayer;
            this.loger = loger;
        }

        /// <summary>
        /// 初始化战报数据
        /// </summary>
        /// <param name="reportData"></param>
        /// <returns></returns>
        public void LoadReportData(JCombatTurnBasedReportData<T> reportData) 
        {
            this.reportData = reportData;
        }

        /// <summary>
        /// 播放战报
        /// </summary>
        /// <returns></returns>
        public async Task Play()
        {
            await Start(new RunableExtraData() { Data = reportData });         
        }

        protected override async void OnStart(RunableExtraData extraData)
        {
            base.OnStart(extraData);

            var reportData = extraData.Data as JCombatTurnBasedReportData<T>;
            if (reportData == null)
                throw new ArgumentException("无效的 JCombatReportData ");

            string winner = reportData.winnerTeamUid;
            //var events = reportData.events;
            var teams = reportData.FormationData;
            //根据战报数据中的队伍信息，初始化游戏对象
            await animationPlayer.Initialize(reportData);
            //用event中的SortIndex字段做升序排序
            reportData.events.Sort((x, y) => x.SortIndex.CompareTo(y.SortIndex));

            if(loger != null)
            {
                //loger.Log($"开始播放战报，胜利队伍: {winner}, 事件数量: {reportData.events.Count} , 第一个战报sortIndex, {reportData.events[0].SortIndex} , 第一个战报castertUid, {reportData.events[0].CastActionUid}");
            }

            await PlayEvents(reportData.events);

            //播放战报结果
            await PlayResult();
        }

        protected abstract Task PlayEvents(List<JCombatTurnBasedEvent> events);

        protected abstract Task PlayResult();


        protected virtual RunableExtraData GetRunableData()
        {
            if (pool == null)
                return new RunableExtraData();

            return pool.Rent<RunableExtraData>();
        }

        protected virtual JCombatTurnBasedEventRunner GetEventRunner()
        {
            if(pool == null)
                return new JCombatTurnBasedEventRunner();

            return pool.Rent<JCombatTurnBasedEventRunner>();
        }

        protected virtual void ReleaseRunner(JCombatTurnBasedEventRunner runner, RunableExtraData extraData)
        {
            if (pool != null)
            {
                runner.Dispose();
                pool.Return(runner);
                pool.Return(extraData);
            }
 
            else
                runner.Dispose();        
        }

       
        public void RePlay()
        {
            LoadReportData(reportData);
            Play();
        }

        public void SetScale(float scale)=> this.scale = scale;
        public float GetScale() => scale;


        protected override void OnStop()
        {
            base.OnStop();
        }


    }
}
