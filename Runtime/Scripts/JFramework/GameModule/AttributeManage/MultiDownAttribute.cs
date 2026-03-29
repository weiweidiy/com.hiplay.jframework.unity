namespace JFramework
{
    public class MultiDownAttribute : ValueAttribute //乘属性
    {
        public MultiDownAttribute(string uid, int id, AttributeType type, float value, AttributeSource systemType, AttributeSubType subType)
            : base(uid, id, type, value, systemType, subType) { }
        public override float GetResult(float origin)
        {
            return origin * (1 - Value / 100f);
        }

    }
}
