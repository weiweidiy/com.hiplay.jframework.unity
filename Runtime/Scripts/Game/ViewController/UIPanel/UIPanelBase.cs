
using Cysharp.Threading.Tasks;
using deVoid.UIFramework;
using Game.Common;
using System;
using UnityEngine;

namespace JFramework.Unity
{
    /// <summary>
    /// UI面板基类，并且会发送面板显示事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UIPanelBase<T> :  APanelController<T> where T : IPanelProperties
    {
        public event Action<UIPanelBase<T>> onPanelEnable;
        public event Action<UIPanelBase<T>> onPanelShow;
        public event Action<UIPanelBase<T>> onPanelHide;
        public event Action<UIPanelBase<T>> onPanelRefresh;

        /// <summary>
        /// 显示面板时调用，在Refresh之前调用
        /// </summary>
        protected virtual void OnPanelEnable() { onPanelEnable?.Invoke(this); }

        /// <summary>
        /// 刷新面板数据
        /// </summary>
        /// <param name="properties"></param>
        protected virtual void OnPanelRefresh(T properties) { onPanelRefresh?.Invoke(this); }

        /// <summary>
        /// 面板显示完成，在Refresh之后调用
        /// </summary>
        protected virtual void OnPanelShow() { onPanelShow?.Invoke(this); }

        /// <summary>
        /// 面板被隐藏时调用
        /// </summary>
        protected virtual void OnPanelHide() { onPanelHide?.Invoke(this); }


        /// <summary>
        /// 面板属性被设置
        /// </summary>
        protected override async void OnPropertiesSet()
        {
            base.OnPropertiesSet();

            OnPanelEnable();

            OnPanelRefresh(Properties);
            // 等待一帧，确保UI位置更新完毕
            await UniTask.Yield();

            OnPanelShow();
        }

        /// <summary>
        /// 隐藏面板时调用
        /// </summary>
        protected override void WhileHiding()
        {
            base.WhileHiding();

            OnPanelHide();
        }

        ///// <summary>
        ///// 发送面板显示事件,在Refresh之后调用
        ///// </summary>
        //protected virtual void OnPanelShow()
        //{
        //    //SendEvent<UIPanelEventShowed>(CreatePanelData());
        //}

        ///// <summary>
        ///// 关闭按钮被点击
        ///// </summary>
        ///// <exception cref="NotImplementedException"></exception>
        //private void OnBtnClosed()
        //{
        //    //SendEvent<UIPanelEventCloseBtnClicked>(CreatePanelData());
        //}

        ////protected EventManager eventManager;
        ////protected override void Awake()
        ////{
        ////    base.Awake();

        ////    //this.Inject();
        ////}

    
        ////public void Initialize(EventManager eventManager)
        ////{
        ////    this.eventManager = eventManager;
        ////}

        ////protected void SendEvent<TEvent>(object arg) where TEvent : Event, new()
        ////{
        ////    eventManager.Raise<TEvent>(arg);
        ////}


    }
}
