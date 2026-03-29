namespace JFramework.Game
{
    public interface IJCombatFormula : IJCombatActionComponent
    {
        /// <summary>
        /// 计算数值
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        void CalcHitValue(IJAttributeableUnit target, ref float value, IJCombatExecutorExecuteArgs executeArgs);

        bool IsHit(IJAttributeableUnit target);
    }
}
