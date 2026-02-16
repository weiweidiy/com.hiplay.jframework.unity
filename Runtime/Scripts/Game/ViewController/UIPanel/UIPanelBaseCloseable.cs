using Game.Common;
using System;
using UnityEngine;

namespace JFramework.Unity
{
    /// <summary>
    /// 可以关闭的UI面板基类，提供了一个关闭按钮，点击后会调用Close方法关闭面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UIPanelBaseCloseable<T> : UIPanelBase<T> where T : IPanelProperties
    {
        public event Action<UIPanelBaseCloseable<T>> onBtnCloseClicked;

        [SerializeField] AdvancedButton btnClose;
        protected override void OnPanelShow()
        {
            base.OnPanelShow();

            if (btnClose != null)
            {
                btnClose.onClick.AddListener(OnBtnCloseClicked);
            }
        }

        protected override void OnPanelHide()
        {
            base.OnPanelHide();

            if (btnClose != null)
            {
                btnClose.onClick.RemoveListener(OnBtnCloseClicked);
            }

        }

        private void OnBtnCloseClicked()
        {
            onBtnCloseClicked?.Invoke(this);
        }
    }
}
