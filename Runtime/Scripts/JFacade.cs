using System;
using System.Threading.Tasks;
using UnityEngine;

namespace JFramework.Unity
{
    public class JFacade : IJUIManager, IJNetworkable
    {
        /// <summary>
        /// UI管理器
        /// </summary>
        IJUIManager uiManager;

        /// <summary>
        /// 网络管理器
        /// </summary>
        IJNetwork networkManager;

        public JFacade(IJUIManager uiManager, IJNetwork networkManager)
        {
            this.uiManager = uiManager;
            this.networkManager = networkManager;
        }


        #region UI接口
        public void CloseWindow(string screenId)
        {
            uiManager.CloseWindow(screenId);
        }

        public Camera GetUICamera()
        {
            return uiManager.GetUICamera();
        }

        public Canvas GetUIFrameCanvas()
        {
            return uiManager.GetUIFrameCanvas();
        }

        public void HidePanel(string screenId)
        {
            uiManager.HidePanel(screenId);
        }


        public bool IsPanelOpen(string screenId)
        {
            return uiManager.IsPanelOpen(screenId);
        }

        public IWindowController OpenWindow(string screenId)
        {
            return uiManager.OpenWindow(screenId);
        }

        public IWindowController OpenWindow<TArg>(string screenId, TArg properties) where TArg : IWindowProperties
        {
            return uiManager.OpenWindow(screenId, properties);
        }

        public IPanelController ShowPanel(string screenId, bool asNew = false)
        {
            return uiManager.ShowPanel(screenId, asNew);
        }

        public IPanelController ShowPanel<TArg>(string screenId, TArg properties, bool asNew = false) where TArg : IPanelProperties
        {
            return uiManager.ShowPanel(screenId, properties, asNew);
        }


        #endregion

        #region 网络接口
        public bool IsConnecting()
        {
            return networkManager.IsConnecting();
        }

        Task<TResponse> IJNetworkable.SendMessage<TResponse>(IJNetMessage pMsg, TimeSpan? timeout)
        {
            return networkManager.SendMessage<TResponse>(pMsg, timeout);
        }

        public Task Connect(string url, string token = null)
        {
            return networkManager.Connect(url, token);
        }

        public void Disconnect()
        {
            networkManager.Disconnect();
        }
        #endregion
    }
}
