using System.Threading.Tasks;

namespace JFramework.Unity
{
    /// <summary>
    /// 场景状态机接口，提供了一个GetContext方法，返回一个BaseSceneSMContext对象，表示场景状态机的上下文信息。实现这个接口的类需要实现GetContext方法来获取场景状态机的上下文信息。
    /// </summary>
    public interface ISceneStateMachineAsync : ISceneSwitchAble
    {
        void SetContext(GameContext context);
    }

    /// <summary>
    /// 可以切换场景状态的接口，提供了一个SwitchToState方法，接受一个状态名称作为参数，返回一个Task对象，表示切换场景状态的异步操作。实现这个接口的类需要实现SwitchToState方法来切换到指定的场景状态。
    /// </summary>
    public interface ISceneSwitchAble
    {
        Task SwitchToState(string stateName);
    }

}
