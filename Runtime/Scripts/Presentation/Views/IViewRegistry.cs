using System;
using System.Collections.Generic;

namespace JFramework.Unity
{
    public interface IViewRegistry
    {
        void RegisterForScene<TScene, TView>() where TView : View, new();

        void RegisterForScene(Type sceneType, View view);

        IReadOnlyList<View> GetViewsForScene<TScene>();

        IReadOnlyList<View> GetViewsForScene(Type sceneType);

        TView GetViewForScene<TScene, TView>() where TView : View;
    }
}
