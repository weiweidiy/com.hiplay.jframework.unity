namespace JFramework.Unity
{
    /// <summary>
    /// Interface that all Windows must implement
    /// </summary>
    public interface IWindowController : IUIScreenController
    {
        bool HideOnForegroundLost { get; }
        bool IsPopup { get; }
        WindowPriority WindowPriority { get; }
    }
}
