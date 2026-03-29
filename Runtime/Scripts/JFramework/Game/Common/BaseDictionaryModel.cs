using System;

namespace JFramework.Game
{
    public abstract class BaseDictionaryModel<TData> : DictionaryContainer<TData>
    {
        protected EventManager eventManager;

        protected BaseDictionaryModel(Func<TData, string> keySelector, EventManager eventManager) : base(keySelector)
        {
            this.eventManager = eventManager;
        }


        public virtual void UpdateData(TData data)
        {
           var key = _keySelector(data);
           var item = Get(key);
            if(item == null)
                Add(data);
            else
                Update(data);
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