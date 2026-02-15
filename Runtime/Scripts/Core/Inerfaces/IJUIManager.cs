using System;
using System.Threading.Tasks;
using UnityEngine;

namespace JFramework.Unity
{
   
    public interface IJUIManager
    {
        /// <summary>
        /// 初始化UI管理器
        /// </summary>
        /// <param name="uiSettingName"></param>
        /// <returns></returns>
        Task Initialize(string uiSettingName);


        /// <summary>
        /// 显示panel
        /// </summary>
        /// <param name="screenId"></param>
        /// <param name="asNew"></param>
        /// <returns></returns>
        IPanelController ShowPanel(string screenId, bool asNew = false);

        /// <summary>
        /// 显示带参数的panel
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="screenId"></param>
        /// <param name="properties"></param>
        /// <param name="asNew"></param>
        /// <returns></returns>
        IPanelController ShowPanel<TArg>(string screenId, TArg properties, bool asNew = false) where TArg : IPanelProperties;

        /// <summary>
        /// 是否打开
        /// </summary>
        /// <param name="screenId"></param>
        /// <returns></returns>
        bool IsPanelOpen(string screenId);

        /// <summary>
        /// 隐藏panel
        /// </summary>
        /// <param name="screenId"></param>
        void HidePanel(string screenId);

        /// <summary>
        /// 打开窗口
        /// </summary>
        /// <param name="screenId"></param>
        /// <returns></returns>
        IWindowController OpenWindow(string screenId);

        /// <summary>
        /// 打开窗口
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="screenId"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        IWindowController OpenWindow<TArg>(string screenId, TArg properties) where TArg : IWindowProperties;

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="screenId"></param>
        void CloseWindow(string screenId);

        Canvas GetUIFrameCanvas();
        Camera GetUICamera();
    }
}

