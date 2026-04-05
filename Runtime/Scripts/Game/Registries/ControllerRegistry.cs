using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JFramework.Unity
{
    public sealed class ControllerRegistry : IControllerRegistry
    {
        private readonly Dictionary<Type, Controller> controllers = new();

        public void Register<TController>() where TController : Controller, new()
        {
            Register(new TController());
        }

        public void Register<TController>(TController controller) where TController : Controller
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            controllers[typeof(TController)] = controller;
        }

        public TController Get<TController>() where TController : Controller
        {
            if (!TryGet(out TController controller))
                throw new InvalidOperationException($"Controller not found: {typeof(TController).FullName}");

            return controller;
        }

        public bool TryGet<TController>(out TController controller) where TController : Controller
        {
            if (controllers.TryGetValue(typeof(TController), out var raw))
            {
                controller = raw as TController;
                return controller != null;
            }

            controller = null;
            return false;
        }

        public Task Do<TController>(GameContext context) where TController : Controller
        {
            return Get<TController>().Do(context);
        }

        public Task Do<TController, TArgs>(GameContext context, TArgs args) where TController : Controller<TArgs>
        {
            return Get<TController>().Do(context, args);
        }
    }
}
