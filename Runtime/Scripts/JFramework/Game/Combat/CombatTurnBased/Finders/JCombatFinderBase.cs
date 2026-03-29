namespace JFramework.Game
{
    public abstract class JCombatFinderBase : JCombatActionComponent, IJCombatTargetsFinder
    {
        protected IJCombatExecutorExecuteArgs executeArgs = new ExecutorExecuteArgs();
        public JCombatFinderBase(float[] args) : base(args)
        {

        }

        protected override int GetValidArgsCount()
        {
            return 0; // 不需要参数
        }

        public abstract IJCombatExecutorExecuteArgs GetTargetsData();

        /// <summary>
        /// 获取目标队伍
        /// </summary>
        /// <returns></returns>
        protected virtual IJCombatTeam GetTargetTeam()
        {
            var myUnitUid = GetOwner().GetCaster();
            var targetTeams = query.GetOppoTeamsByUnit(myUnitUid);
            var targetTeam = targetTeams[0];// 默认选择第一个敌对队伍
            return targetTeam;
        }

        protected override void OnStart(RunableExtraData extraData)
        {
            base.OnStart(extraData);

            // 设置执行参数的施法者
            executeArgs.CasterUnit = query.GetUnit(GetOwner().GetCaster()) as IJCombatCasterUnit;
        }
    }
}
