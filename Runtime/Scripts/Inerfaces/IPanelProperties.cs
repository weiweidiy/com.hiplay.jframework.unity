namespace JFramework.Unity
{
    /// <summary>
    /// Base interface for all Panel properties
    /// </summary>
    public interface IPanelProperties : IScreenProperties
    {
        PanelPriority Priority { get; set; }
    }
}
