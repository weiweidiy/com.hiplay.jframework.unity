namespace JFramework.Unity
{
    /// <summary>
    /// 默认的场景状态机上下文类，包含一个IJFacade类型的属性Facade，表示场景状态机的外观接口。这个类可以作为场景状态机的上下文信息，提供给状态机中的状态使用，以便它们能够访问外观接口来获取资源加载器和UI管理器等功能。
    /// </summary>
    public class GameContext
    {
        public IJFacade Facade { get; set; }

    }
}
