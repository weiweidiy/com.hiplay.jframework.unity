namespace JFramework
{
    public class MinusAttribute : ValueAttribute
    {
        public MinusAttribute(string uid, int id, AttributeType type, float value, AttributeSource systemType, AttributeSubType subType)
            : base(uid, id, type, value, systemType, subType) { }

        public override float GetResult(float origin)
        {
            //to do ： 溢出如何处理
            return origin - Value;
        }

    }
}
