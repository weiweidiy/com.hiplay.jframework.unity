namespace JFramework
{
    /// <summary>
    /// 通用属性数据
    /// </summary>
    public class AttributeGeneratorData
    {
        public int AttributeId;
        public float AttributeValue;
        public AttributeSource SysSource;


        public AttributeGeneratorData(int pID, float pValue, AttributeSource sysSource = AttributeSource.Null)
        {
            AttributeId = pID;
            AttributeValue = pValue;
            SysSource = sysSource;
        }
    }
}
