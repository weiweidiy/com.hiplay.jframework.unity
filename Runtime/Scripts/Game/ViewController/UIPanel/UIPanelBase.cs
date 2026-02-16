
using Cysharp.Threading.Tasks;
using deVoid.UIFramework;
using Game.Common;
using System;
using UnityEngine;

namespace JFramework.Unity
{

    /// <summary>
    /// UI面板基类，带有关闭按钮,并且会发送面板显示事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UIPanelBase<T> :  APanelController<T> where T : IPanelProperties
    {
        /// <summary>
        /// 关闭按钮
        /// </summary>
        [SerializeField] protected AdvancedButton btnClose;

        /// <summary>
        /// panel数据基类
        /// </summary>
        public abstract class UIPanelBaseData
        {
            public UIPanelBase<T> panel;
        }

        /// <summary>
        /// 显示面板时调用，在Refresh之前调用
        /// </summary>
        protected virtual void OnPanelEnable() { }

        /// <summary>
        /// 面板被隐藏时调用
        /// </summary>
        protected virtual void OnPanelHide() { }

        /// <summary>
        /// 刷新面板数据
        /// </summary>
        /// <param name="properties"></param>
        protected abstract void OnPanelRefresh(T properties);

        /// <summary>
        /// 创建一个PanelData对象
        /// </summary>
        /// <returns></returns>
        protected abstract UIPanelBaseData CreatePanelData();

        /// <summary>
        /// 面板属性被设置
        /// </summary>
        protected override async void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            if (btnClose != null)
            {
                btnClose.onClick.RemoveAllListeners();
                btnClose.onClick.AddListener(OnBtnClosed);
            }

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
            if (btnClose != null)
            {
                btnClose.onClick.RemoveAllListeners();
            }

            OnPanelHide();
        }

        /// <summary>
        /// 发送面板显示事件,在Refresh之后调用
        /// </summary>
        protected virtual void OnPanelShow()
        {
            //SendEvent<UIPanelEventShowed>(CreatePanelData());
        }

        /// <summary>
        /// 关闭按钮被点击
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnBtnClosed()
        {
            //SendEvent<UIPanelEventCloseBtnClicked>(CreatePanelData());
        }

        protected EventManager eventManager;
        protected override void Awake()
        {
            base.Awake();

            //this.Inject();
        }

    
        public void Initialize(EventManager eventManager)
        {
            this.eventManager = eventManager;
        }

        protected void SendEvent<TEvent>(object arg) where TEvent : Event, new()
        {
            eventManager.Raise<TEvent>(arg);
        }


    }
}
