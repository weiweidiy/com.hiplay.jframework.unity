namespace JFramework
{
    public abstract class ValueAttribute : BaseAttribute
    {
        public AttributeSubType SubType;
        //public string Uid { get; private set; }
        //public int Id { get; private set; }

        public float Value { get; private set; }
        public AttributeSource SysSource { get; private set; }


        public ValueAttribute(string uid, int id, AttributeType type, float value, AttributeSource sysSource, AttributeSubType subType) : base(uid, id, type)
        {
            //var cfg = AttributeCfg.GetData(id);
            //this.Type = type; // (AttributeType)cfg.Type;
            Value = value;
            //this.Id = id;
            //this.Uid = Uid;
            SysSource = sysSource;
            SubType = subType;
        }

        public virtual float GetResult(float origin) => 0f;
        public void UpdateValue(float value)
        {
            Value = value;
        }

    }
}
