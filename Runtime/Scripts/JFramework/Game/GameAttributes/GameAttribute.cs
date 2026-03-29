using System;
using System.Collections.Generic;

namespace JFramework.Game
{

    /// <summary>
    /// 屬性抽象類
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public abstract class GameAttribute<T> : IUnique where T : struct
    {
        /// <summary>
        /// 原始值
        /// </summary>
        public T OriginValue { get; private set; }

        /// <summary>
        /// 当前值
        /// </summary>
        protected T curValue;
        public abstract T CurValue { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public T MaxValue { get; protected set; }

        public string Uid { get; private set; }

        /// <summary>
        /// 额外属性值
        /// </summary>
        protected Dictionary<string, T> extraAttributes;

        public GameAttribute(string uid, T value, T maxValue)
        {
            Uid = uid;
            OriginValue = value;
            curValue = value;
            MaxValue = maxValue;
            extraAttributes = new Dictionary<string, T>();
        }

        public abstract T GetAllExtraValue();


        /// <summary>
        /// 添加一个加成值
        /// </summary>
        /// <param name="extraUid">来源者的uid，便于删除</param>
        /// <param name="value"></param>
        public abstract void AddExtraValue(string extraUid, T value);


        /// <summary>
        /// 移除一个加成值
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool RemoveExtraValue(string uid)
        {
            return extraAttributes.Remove(uid);
        }

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool MinusExtraValue(string uid, T value);

        public abstract T Plus(T value);
        public abstract T Minus(T value);
        /// <summary>
        /// 乘法，直接乘的
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract T Multi(T value);
        public abstract T Div(T value);
        public abstract T PlusMax(T value);
        public abstract T MinusMax(T value);
        public abstract T MultiMax(T value);
        public abstract T DivMax(T value);
        public void Reset()
        {
            curValue = OriginValue;
            extraAttributes.Clear();
        }

        public abstract bool IsMax();


    }
}