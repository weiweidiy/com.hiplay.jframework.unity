using System;

namespace JFramework.Game
{
    public class GameAttributeFloat : GameAttribute<float>
    {
        public override float CurValue { get { return curValue + GetAllExtraValue(); } set => curValue = value; }
        public GameAttributeFloat(string uid, float value, float maxValue) : base(uid, value, maxValue)
        {
        }

        public override float Plus(float value)
        {
            CurValue += value;
            CurValue = Math.Min(CurValue, MaxValue);
            return CurValue;
        }

        public override float PlusMax(float value)
        {
            MaxValue += value;
            return MaxValue;
        }

        public override float Minus(float value)
        {
            CurValue -= value;
            CurValue = Math.Max(CurValue, 0);
            return CurValue;
        }

        public override float MinusMax(float value)
        {
            MaxValue -= value;
            MaxValue = Math.Max(MaxValue, 0);
            CurValue = Math.Min(CurValue, MaxValue);
            return MaxValue;
        }

        public override float Multi(float value)
        {
            CurValue *= value;
            CurValue = Math.Min(CurValue, MaxValue);
            return CurValue;
        }

        public override float MultiMax(float value)
        {
            MaxValue *= value;
            return MaxValue;
        }

        public override float Div(float value)
        {
            if (value == 0)
                throw new ArgumentException("除數不能為0");

            CurValue = CurValue / value;
            return CurValue;
        }

        public override float DivMax(float value)
        {
            if (value == 0)
                throw new ArgumentException("除數不能為0");

            MaxValue = MaxValue / value;
            CurValue = Math.Min(CurValue, MaxValue);
            return MaxValue;
        }

        public override bool IsMax()
        {
            return CurValue == MaxValue;
        }

        public override float GetAllExtraValue()
        {
            float result = 0;

            foreach (var extraValue in extraAttributes)
            {
                result += extraValue.Value;
            }

            return result;
        }

        public override void AddExtraValue(string extraUid, float value)
        {
            if (extraAttributes.ContainsKey(extraUid))
            {
                extraAttributes[extraUid] += value;
            }
            else
                extraAttributes.Add(extraUid, value);
        }

        public override bool MinusExtraValue(string extraUid, float value)
        {
            if (extraAttributes.ContainsKey(extraUid))
            {
                extraAttributes[extraUid] -= value;
                return true;
            }
            else
                return false;
        }
    }
}