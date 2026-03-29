namespace JFramework.Game
{
    /// <summary>
    /// 改变值公式，带有回合数限制
    /// </summary>
    public class JCombatFormulaChangeValueExtraWithTurn : JCombatFormulaChangeValue
    {
        public JCombatFormulaChangeValueExtraWithTurn(float[] args) : base(args)
        {
        }

        protected override int GetValidArgsCount()
        {
            return base.GetValidArgsCount() + 2;
        }

        int GetTurnCount()
        {
            return (int)GetArg(2);
        }

        float GetExtraValue()
        {
            return GetArg(3);
        }

        protected override float GetCalcValueArg(IJAttributeableUnit target, IJCombatExecutorExecuteArgs executeArgs)
        {
            var curFrame = query.GetCurFrame();
            if(curFrame == GetTurnCount()) // 如果当前回合数等于指定的回合数
            {
                return GetExtraValue();
            }
            else
            {
                return base.GetCalcValueArg(target, executeArgs);
            }
        }


    }
}
