using System;
using System.Collections.Generic;

namespace JFramework.Unity
{
    public sealed class ViewRegistry : IViewRegistry
    {
        private readonly Dictionary<Type, List<View>> viewsByScene = new();

        public void RegisterForScene<TScene, TView>() where TView : View, new()
        {
            RegisterForScene(typeof(TScene), new TView());
        }

        /// <summary>
        /// 给指定场景注册一个视图实例，视图实例将在场景状态进入时被打开，在场景状态退出时被关闭
        /// </summary>
        /// <param name="sceneType"></param>
        /// <param name="view"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void RegisterForScene(Type sceneType, View view)
        {
            if (sceneType == null)
                throw new ArgumentNullException(nameof(sceneType));

            if (view == null)
                throw new ArgumentNullException(nameof(view));

            if (!viewsByScene.TryGetValue(sceneType, out var views))
            {
                views = new List<View>();
                viewsByScene[sceneType] = views;
            }

            views.Add(view);
        }

        /// <summary>
        /// 获取所有注册到指定场景的视图实例，如果没有注册任何视图，则返回一个空列表
        /// </summary>
        /// <typeparam name="TScene"></typeparam>
        /// <returns></returns>
        public IReadOnlyList<View> GetViewsForScene<TScene>()
        {
            return GetViewsForScene(typeof(TScene));
        }

        /// <summary>
        /// 获取所有注册到指定场景的视图实例，如果没有注册任何视图，则返回一个空列表
        /// </summary>
        /// <param name="sceneType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IReadOnlyList<View> GetViewsForScene(Type sceneType)
        {
            if (sceneType == null)
                throw new ArgumentNullException(nameof(sceneType));

            if (viewsByScene.TryGetValue(sceneType, out var views))
                return views;

            return Array.Empty<View>();
        }

        public TView GetViewForScene<TScene, TView>() where TView : View
        {
            var views = GetViewsForScene<TScene>();
            foreach (var view in views)
            {
                if (view is TView typedView)
                    return typedView;
            }
            return null;
        }
    }
}
