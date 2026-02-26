
using System;

namespace JFramework.Unity
{

    /// <summary>
    /// Interface that all Panels must implement
    /// </summary>
    public interface IPanelController : IUIScreenController
    {
        PanelPriority Priority { get; }
    }
}
