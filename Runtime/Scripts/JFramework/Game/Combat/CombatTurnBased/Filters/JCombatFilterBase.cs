namespace JFramework.Game
{
    public abstract class JCombatFilterBase : JCombatActionComponent, IJCombatFilter
    {
        public JCombatFilterBase(float[] args) : base(args)
        {
        }

        protected override int GetValidArgsCount()
        {
            return 0;
        }

        public abstract bool Filter(IJCombatExecutorExecuteArgs executeArgs, IJCombatCasterTargetableUnit target);
    }
}
