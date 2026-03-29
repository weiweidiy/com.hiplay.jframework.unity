namespace JFramework.Game
{
    public interface IJCombatFilter : IJCombatActionComponent
    {
        bool Filter(IJCombatExecutorExecuteArgs executeArgs, IJCombatCasterTargetableUnit target);
    }
}
