using JFramework.Unity;
using System.Collections.Generic;

namespace JFramework.Unity
{
    public abstract class BaseControllerManager : IControllerManager
    {
        protected Dictionary<string, Controller> controllers = new();

        public abstract void RegisterControllers();

        public Controller GetController(string name)
        {
            controllers.TryGetValue(name, out var controller);
            return controller;
        }
    }

}