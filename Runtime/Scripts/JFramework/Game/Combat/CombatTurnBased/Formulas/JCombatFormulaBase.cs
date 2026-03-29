namespace JFramework.Game
{
    public abstract class JCombatFormulaBase : JCombatActionComponent, IJCombatFormula
    {
        protected JCombatFormulaBase(float[] args) : base(args)
        {
        }

        public virtual bool IsHit(IJAttributeableUnit target)
        {
            // 默认实现，子类可以覆盖
            return true;
        }

        public abstract void CalcHitValue(IJAttributeableUnit target, ref float value, IJCombatExecutorExecuteArgs executeArgs);
    }
}
