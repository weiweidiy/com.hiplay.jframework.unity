using System;
using System.Collections.Generic;
using System.Text;

namespace JFramework
{
    public abstract class AdItem : IAdItem
    {
        /// <summary>
        /// 广告事件委托
        /// </summary>
        protected Action<bool> _closeCallBack = null;
        protected Action _clickCallBack = null;
        protected Action<string> _loadComplete = null;

        /// <summary>
        /// 广告位代码
        /// </summary>
        public string AdCode { get; private set; }

        private AdItem() { }

        public AdItem(string adCode)
        {
            AdCode = adCode;
        }

        public AdItem(string adCode, Action<string> loadComplete) : this(adCode)
        {
            _loadComplete = loadComplete;
        }

        /// <summary>
        /// 被点击
        /// </summary>
        public virtual void OnClick()
        {
            _clickCallBack?.Invoke();
        }

        /// <summary>
        /// 加载完成
        /// </summary>
        public virtual void OnLoadComplete()
        {
            _loadComplete?.Invoke(AdCode);
        }

        /// <summary>
        /// 子类重写，实现删除广告对象
        /// </summary>
        /// <param name="result"></param>
        public abstract void Clear();

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="result"></param>
        public void Close(bool result)
        {
            Clear();

            _closeCallBack?.Invoke(result);
        }

        /// <summary>
        /// 显示广告
        /// </summary>
        protected abstract void Show();

        /// <summary>
        /// 实现接口
        /// </summary>
        /// <param name="closeCallBack"></param>
        /// <param name="clickCallBack"></param>
        public void Show(Action<bool> closeCallBack = null, Action clickCallBack = null)
        {
            _clickCallBack = clickCallBack;
            _closeCallBack = closeCallBack;

            Show();
        }


        #region 抽象接口实现
        public abstract AdType AdType { get; }
        public abstract bool IsReady();
        public abstract void Load();

        #endregion
    }
}
