
using System;

namespace JFramework.Game
{
    /// <summary>
    /// 模型基类，提供了消息发送功能
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public abstract class BaseModel<TData>
    {
        /// <summary>
        /// 值对象,子类可以修改值
        /// </summary>
        protected TData data;
        public TData Data => data;

        /// <summary>
        /// 消息系统，负责发送消息变化
        /// </summary> 
        protected EventManager eventManager;

        public BaseModel(EventManager eventManager)
        {
            if (eventManager == null)
                throw new Exception(this.GetType().ToString() + "inject eventManager failed, it is null !");

            this.eventManager = eventManager;
            //Uid = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 初始化模型
        /// </summary>
        /// <param name="vo"></param>
        public void Initialize(TData vo)
        {
            this.data = vo;
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