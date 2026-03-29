namespace JFramework.Game
{
    public class JCombatExecutorAttributeDataChange : JCombatExecutorBase
    {
        public JCombatExecutorAttributeDataChange(IJCombatContext context, IJCombatFilter filter, IJCombatTargetsFinder finder, IJCombatFormula formulua, float[] args) : base(context, filter, finder, formulua, args)
        {
        }
        protected override int GetValidArgsCount()
        {
            return 1; // 只需要一个参数，通常是属性值或相关系数
        }

        string GetTargetAttributeName()
        {
            var arg = (int)GetArg(0);

            return query.GetAttrName(arg);
        }

        protected override IJCobmatExecuteArgsHistroy DoExecute(IJCombatExecutorExecuteArgs executeArgs, IJCombatCasterTargetableUnit target)
        {
            var targetAttribute = target.GetAttribute(GetTargetAttributeName()) as GameAttributeInt;
            var value = (float)targetAttribute.CurValue;
            formulua.CalcHitValue(target, ref value, executeArgs); 
            targetAttribute.CurValue = (int)value;
            return new JCombatExecutorExecuteArgsHistroy() {  };
        }
    }
}
