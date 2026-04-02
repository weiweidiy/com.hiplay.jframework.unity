
using deVoid.UIFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;



namespace JFramework.Unity
{
    /// <summary>
    /// 默认的UI管理器，采用第三方UI框架devoid UIFramework实现，负责ui的注册和显示等功能
    /// </summary>
    public class DefaultUIManager : IJUIManager
    {
        /// <summary>
        /// 资源加载器
        /// </summary>
        IAssetsLoader assetsLoader;

        /// <summary>
        /// ui框架实例，采用第三方UI框架devoid UIFramework，负责ui的注册和显示等功能
        /// </summary>
        UIFrame uiFrame;

        /// <summary>
        /// 注册的ui预制体
        /// </summary>
        UISettings uiSettings;

        class RuntimeScreenRegistration
        {
            public GameObject Instance;
            public IUIScreenController Controller;
        }

        readonly Dictionary<string, RuntimeScreenRegistration> runtimeScreens = new Dictionary<string, RuntimeScreenRegistration>();


        public DefaultUIManager(IAssetsLoader assetsLoader)
        {
            if (assetsLoader == null)
                throw new Exception(this.GetType().ToString() + " Inject IAssetsLoader failed!");

            this.assetsLoader = assetsLoader;
        }

        /// <summary>
        /// 初始化,根据场景初始化
        /// </summary>
        public async Task Initialize(string uiSettingName)
        {
            try
            {
                uiSettings = await assetsLoader.LoadAssetAsync<UISettings>(uiSettingName);
                uiFrame = uiSettings.CreateUIInstance();
                runtimeScreens.Clear();

                var mainCamera = Camera.main;
                if (mainCamera != null)
                {
                    var cameraData = mainCamera.GetUniversalAdditionalCameraData();
                    if (cameraData != null && !cameraData.cameraStack.Contains(uiFrame.UICamera))
                    {
                        cameraData.cameraStack.Add(uiFrame.UICamera);
                    }
                }
            }
            catch
            {
                Debug.LogError("Failed to initialize UIManager with settings: " + uiSettingName);
                throw;
            }
        }

        /// <summary>
        /// 注册ui,预制体需要在uisetting里注册
        /// </summary>
        /// <param name="screenId"></param>
        /// <param name="controller"></param>
        /// <param name="screenTransform"></param>
        RuntimeScreenRegistration CreateScreenFromUISettings(string screenId)
        {
            //需要新的实例
            var screen = GetScreenGameObjectPrefab(screenId);
            if (screen == null)
                throw new System.Exception("ui没有注册，请在uisettings中注册ui预制体：" + screenId);

            //用新的screenid注册prefab
            var newScreenId = Guid.NewGuid().ToString();
            var screenInstance = GameObject.Instantiate(screen);
            var screenController = screenInstance.GetComponent<IUIScreenController>();
            if (screenController != null)
            {
                Debug.Log("---- RegisterScreen  --------- " + screen.name);
                uiFrame.RegisterScreen(newScreenId, screenController, screenInstance.transform);
                if (screenInstance.activeSelf)
                {
                    screenInstance.SetActive(false);
                }

                var runtimeRegistration = new RuntimeScreenRegistration()
                {
                    Instance = screenInstance,
                    Controller = screenController,
                };
                runtimeScreens[newScreenId] = runtimeRegistration;
                return runtimeRegistration;
            }
            else
            {
                GameObject.Destroy(screenInstance);
                throw new System.Exception("[UIConfig] Screen doesn't contain a ScreenController! Skipping " + screen.name);
            }
        }


        IPanelController IJUIManager.ShowPanel(string screenId, bool asNew)
        {
            if (uiFrame.IsPanelOpen(screenId) && asNew)
            {
                var runtimeRegistration = CreateScreenFromUISettings(screenId);
                return uiFrame.ShowPanel(runtimeRegistration.Controller.ScreenId);
            }
            return uiFrame.ShowPanel(screenId);
        }

        /// <summary>
        /// 显示panel
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="screenId"></param>
        /// <param name="properties"></param>
        public IPanelController ShowPanel<TArg>(string screenId, TArg properties, bool asNew = false) where TArg : IPanelProperties
        {
            if (uiFrame.IsPanelOpen(screenId) && asNew)
            {
                var runtimeRegistration = CreateScreenFromUISettings(screenId);
                return uiFrame.ShowPanel(runtimeRegistration.Controller.ScreenId, properties);
            }

            return uiFrame.ShowPanel(screenId, properties);
        }


        public bool IsPanelOpen(string screenId)
        {
            return uiFrame.IsPanelOpen(screenId);
        }

        /// <summary>
        /// 销毁一个ui(unregist)
        /// </summary>
        /// <param name="screenId"></param>
        public void DestroyPanel(string screenId)
        {
            if (!runtimeScreens.TryGetValue(screenId, out var runtimeRegistration))
            {
                return;
            }

            uiFrame.UnregisterPanel(screenId, runtimeRegistration.Controller as IPanelController);
            runtimeScreens.Remove(screenId);
            GameObject.Destroy(runtimeRegistration.Instance);
        }

        /// <summary>
        /// 关闭Panel
        /// </summary>
        /// <param name="screenId"></param>
        public void HidePanel(string screenId)
        {
            uiFrame.HidePanel(screenId);
            DestroyPanel(screenId);
        }

        //public void OpenWindow(string screenId)
        //{
        //    uiFrame.OpenWindow(screenId);
        //}

        //public void OpenWindow<TArg>(string screenId, TArg properties) where TArg : IWindowProperties
        //{
        //    uiFrame.OpenWindow(screenId, properties);
        //}

        IWindowController IJUIManager.OpenWindow(string screenId)
        {
            var runtimeRegistration = CreateScreenFromUISettings(screenId);
            var controller = runtimeRegistration.Controller as IWindowController;
            if (controller == null)
            {
                DestroyRuntimeScreen(runtimeRegistration.Controller.ScreenId);
                throw new InvalidOperationException($"Screen {screenId} is not a window.");
            }

            uiFrame.OpenWindow(runtimeRegistration.Controller.ScreenId);
            return controller;
        }

        IWindowController IJUIManager.OpenWindow<TArg>(string screenId, TArg properties)
        {
            var runtimeRegistration = CreateScreenFromUISettings(screenId);
            var controller = runtimeRegistration.Controller as IWindowController;
            if (controller == null)
            {
                DestroyRuntimeScreen(runtimeRegistration.Controller.ScreenId);
                throw new InvalidOperationException($"Screen {screenId} is not a window.");
            }

            uiFrame.OpenWindow(runtimeRegistration.Controller.ScreenId, properties);
            return controller;
        }

        public void CloseWindow(string screenId)
        {
            uiFrame.CloseWindow(screenId);
            DestroyRuntimeScreen(screenId);
        }

        /// <summary>
        /// 获取注册的预制体
        /// </summary>
        /// <param name="screenId"></param>
        /// <returns></returns>
        GameObject GetScreenGameObjectPrefab(string screenId)
        {
            return uiSettings.screensToRegister.Where(i => i.name == screenId).SingleOrDefault();
        }

        void DestroyRuntimeScreen(string screenId)
        {
            if (!runtimeScreens.TryGetValue(screenId, out var runtimeRegistration))
            {
                return;
            }

            if (runtimeRegistration.Controller is IWindowController windowController)
            {
                uiFrame.UnregisterWindow(screenId, windowController);
            }
            else if (runtimeRegistration.Controller is IPanelController panelController)
            {
                uiFrame.UnregisterPanel(screenId, panelController);
            }

            runtimeScreens.Remove(screenId);
            GameObject.Destroy(runtimeRegistration.Instance);
        }

        public Canvas GetUIFrameCanvas()
        {
            return uiFrame.GetComponent<Canvas>();
        }

        public Camera GetUICamera()
        {
            return uiFrame.UICamera;
        }


    }
}
