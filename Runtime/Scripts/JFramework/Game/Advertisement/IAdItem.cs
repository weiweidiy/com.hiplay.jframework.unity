using System;

namespace JFramework
{
    /// <summary>
    /// 广告项目接口
    /// </summary>
    public interface IAdItem
    {
        /// <summary>
        /// 获取广告类型： Banner等等
        /// </summary>
        AdType AdType { get; }

        /// <summary>
        /// 广告位代码
        /// </summary>
        string AdCode { get; }

        /// <summary>
        /// 加载广告
        /// </summary>
        void Load();

        /// <summary>
        /// 是否完成加载
        /// </summary>
        /// <returns></returns>
        bool IsReady();

        /// <summary>
        /// 显示广告
        /// </summary>
        /// <param name="closeCallBack"></param>
        /// <param name="clickCallBack"></param>
        void Show(Action<bool> closeCallBack = null, Action clickCallBack = null);

        /// <summary>
        /// 关闭广告
        /// </summary>
        void Close(bool result);
    }
}
