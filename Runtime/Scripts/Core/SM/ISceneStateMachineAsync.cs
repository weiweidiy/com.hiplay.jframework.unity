using System.Threading.Tasks;

namespace JFramework.Unity
{
    public interface ISceneStateMachineAsync
    {
        Task SwitchToState(string stateName);
    }
}
