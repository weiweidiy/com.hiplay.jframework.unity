using System;

namespace JFramework.Game
{
    public class GameAttributeInt : GameAttribute<int>
    {
        public override int CurValue { get { return curValue + GetAllExtraValue(); } set => curValue = value; }
        public GameAttributeInt(string uid, int value, int maxValue) : base(uid, value, maxValue)
        {
        }

        public override int Plus(int value)
        {
            CurValue += value;
            CurValue = Math.Min(CurValue, MaxValue);
            return CurValue;
        }

        public override int PlusMax(int value)
        {
            MaxValue += value;
            return MaxValue;
        }

        public override int Minus(int value)
        {
            CurValue -= value;
            CurValue = Math.Max(CurValue, 0);
            return CurValue;
        }

        public override int MinusMax(int value)
        {
            MaxValue -= value;
            MaxValue = Math.Max(MaxValue, 0);
            CurValue = Math.Min(CurValue, MaxValue);
            return MaxValue;
        }

        public override int Multi(int value)
        {
            CurValue *= value;
            CurValue = Math.Min(CurValue, MaxValue);
            return CurValue;
        }

        public override int MultiMax(int value)
        {
            MaxValue *= value;
            return MaxValue;
        }

        public override int Div(int value)
        {
            if (value == 0)
                throw new ArgumentException("除數不能為0");

            CurValue = CurValue / value;
            return CurValue;
        }

        public override int DivMax(int value)
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

        public override int GetAllExtraValue()
        {
            int result = 0;

            foreach (var extraValue in extraAttributes)
            {
                result += extraValue.Value;
            }

            return result;
        }

        public override void AddExtraValue(string extraUid, int value)
        {
            if (extraAttributes.ContainsKey(extraUid))
            {
                extraAttributes[extraUid] += value;
            }
            else
                extraAttributes.Add(extraUid, value);
        }

        public override bool MinusExtraValue(string extraUid, int value)
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