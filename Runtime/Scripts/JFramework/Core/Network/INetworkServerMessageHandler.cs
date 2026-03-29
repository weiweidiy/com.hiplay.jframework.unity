//using System.Reactive.Linq;
//using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace JFramework
{
    public interface INetworkServerMessageHandler
    {
        Task<IJNetMessage> Handle(IJNetMessage message);
    }
}
