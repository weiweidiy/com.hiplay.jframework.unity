namespace JFramework.Game
{
    /// <summary>
    /// 筛选器-指定回合数
    /// </summary>
    public class JCombatFilterTurnNumber : JCombatFilterBase
    {
        public JCombatFilterTurnNumber(float[] args) : base(args)
        {
        }

        protected override int GetValidArgsCount()
        {
            return base.GetValidArgsCount() + 1;
        }

        int GetTargetTurnNumber()
        {
            return (int)GetArg(0);
        }

        public override bool Filter(IJCombatExecutorExecuteArgs executeArgs, IJCombatCasterTargetableUnit target)
        {
            var turn = query.GetCurFrame();
            if (turn != GetTargetTurnNumber())
            {
                return false;
            }
            return true;
        }
    }
}
