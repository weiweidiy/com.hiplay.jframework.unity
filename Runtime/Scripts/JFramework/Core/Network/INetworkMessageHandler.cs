//using System.Reactive.Linq;
//using System.Reactive.Subjects;
namespace JFramework
{
    public interface INetworkMessageHandler
    {
        void Handle(IJNetMessage message);
    }
}
