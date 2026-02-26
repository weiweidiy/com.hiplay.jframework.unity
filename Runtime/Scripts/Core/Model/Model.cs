using System;
using System.Collections.Generic;

///游戏可以服用
namespace JFramework.Unity
{
    /// <summary>
    /// 基础model，基于字典查询，提供事件发送功能，允许在不同的场景状态中复用同一个model
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public abstract class Model<TData> : DictionaryContainer<TData>
    {
        EventManager eventManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="keySelector">用于查询唯一对象用的</param>
        /// <param name="eventManager"></param>
        protected Model(Func<TData, string> keySelector, EventManager eventManager) : base(keySelector)
        {
            this.eventManager = eventManager;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="lstData"></param>
        public void Initialize(List<TData> lstData)
        {
            // 清空当前数据
            Clear();
            // 批量添加新数据
            AddRange(lstData);

            //Debug.Log("Model Initialize " + this.GetType());
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        protected void SendEvent<T>(object arg) where T : Event, new()
        {
            eventManager.Raise<T>(arg);
        }
    }
}
