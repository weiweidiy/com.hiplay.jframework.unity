using System.Threading.Tasks;

namespace JFramework.Unity
{
    public interface IControllerRegistry
    {
        void Register<TController>() where TController : Controller, new();

        void Register<TController>(TController controller) where TController : Controller;

        TController Get<TController>() where TController : Controller;

        bool TryGet<TController>(out TController controller) where TController : Controller;

        Task Do<TController>(GameContext context) where TController : Controller;

        Task Do<TController, TArgs>(GameContext context, TArgs args) where TController : Controller<TArgs>;
    }
}
