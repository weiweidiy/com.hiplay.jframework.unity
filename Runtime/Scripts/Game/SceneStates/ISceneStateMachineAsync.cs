using System.Threading.Tasks;

namespace JFramework.Unity
{
    /// <summary>
    /// 场景状态机接口，提供了一个GetContext方法，返回一个BaseSceneSMContext对象，表示场景状态机的上下文信息。实现这个接口的类需要实现GetContext方法来获取场景状态机的上下文信息。
    /// </summary>
    public interface ISceneStateMachineAsync 
    {
        Task SwitchToState(string stateName, GameContext context);
    }



}
