using JFramework.Unity;
using System.Collections.Generic;

namespace JFramework.Unity
{
    public abstract class BaseViewControllerManager : IViewControllerManager
    {
        protected Dictionary<string, List<View>> viewControllers = new Dictionary<string, List<View>>();
        public abstract void RegisterViewControllers();
        public View[] GetViewControllers(string group)
        {
            var ctrollers = viewControllers[group];

            return ctrollers.ToArray();
        }

        public View GetViewController(string name)
        {
            foreach (var group in viewControllers.Values)
            {
                foreach (var controller in group)
                {
                    if (controller.Name == name)
                    {
                        return controller;
                    }
                }
            }
            return null;
        }
    }

}