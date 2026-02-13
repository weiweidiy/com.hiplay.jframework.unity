using System;
using System.Threading.Tasks;
using UnityEngine;

namespace JFramework.Unity
{
    public class JFacade : IJUIManager 
    {
        IJUIManager uiManager;

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

        public Task Connect(string url, string token = null)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
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

        public bool IsConnecting()
        {
            throw new NotImplementedException();
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
    }
}
