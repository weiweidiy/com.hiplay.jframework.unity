
using System;

namespace JFramework.Unity
{
    public interface IUIScreenController
    {
        string ScreenId { get; set; }
        bool IsVisible { get; }

        void Show(IScreenProperties props = null);
        void Hide(bool animate = true);

        Action<IUIScreenController> InTransitionFinished { get; set; }
        Action<IUIScreenController> OutTransitionFinished { get; set; }
        Action<IUIScreenController> CloseRequest { get; set; }
        Action<IUIScreenController> ScreenDestroyed { get; set; }
    }
}
