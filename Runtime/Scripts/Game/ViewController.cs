
using JFramework;
using System;

///游戏可以服用
namespace JFramework.Unity
{
    public abstract class ViewController
    {
        public class Open : Event { }
        public class Refresh : Event { }
        public class Close : Event { }

        public class ControllerBaseArgs
        {
            //通用参数
            public string panelName; //面板名字
        }

        protected EventManager eventManager;


        protected void SendEvent<T>(object arg) where T : Event, new()
        {
            eventManager.Raise<T>(arg);
        }

        public ViewController(EventManager eventManager) 
        {
            this.eventManager = eventManager;
        }

        /// <summary>
        /// 在state onenter中被调用，注册事件监听
        /// </summary>
        public virtual void OnStart()
        {
            eventManager.AddListener<Open>(DoOpen);
            eventManager.AddListener<Close>(DoClose);
            eventManager.AddListener<Refresh>(DoRefresh);
        }

        public virtual void OnStop()
        {
            eventManager.RemoveListener<Open>(DoOpen);
            eventManager.RemoveListener<Close>(DoClose);
            eventManager.RemoveListener<Refresh>(DoRefresh);
        }


        protected abstract void DoOpen(Open e);

        protected virtual void DoClose(Close e) { }

        protected virtual void DoRefresh(Refresh e) { }
    }
}
