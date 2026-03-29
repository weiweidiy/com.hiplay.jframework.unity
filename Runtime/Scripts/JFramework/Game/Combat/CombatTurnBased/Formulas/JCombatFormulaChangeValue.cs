namespace JFramework.Game
{
    /// <summary>
    /// 执行器会使用 CalcHitValue 方法来计算最终值
    /// </summary>
    public class JCombatFormulaChangeValue : JCombatFormulaBase
    {
        public JCombatFormulaChangeValue(float[] args) : base(args)
        {
        }

        protected override int GetValidArgsCount()
        {
            return 2;
        }

        int GetCalcMode()
        {
            return (int)GetArg(0);
        }

        protected virtual float GetCalcValueArg(IJAttributeableUnit target, IJCombatExecutorExecuteArgs executeArgs)
        {
            return GetArg(1);
        }

        public override void CalcHitValue(IJAttributeableUnit target, ref float value, IJCombatExecutorExecuteArgs executeArgs)
        {
            if(GetCalcMode() == 0) 
            {
                value =  value * GetCalcValueArg(target, executeArgs); // 计算伤害提升百分比
            }
            else if(GetCalcMode() == 1)
            {
                value = value + GetCalcValueArg(target, executeArgs); // 计算伤害提升绝对值
            }
            else
            {
                throw new System.Exception("JCombatFormulaDamageDataChange: Invalid calculation mode.");
            }
        }


    }
}
