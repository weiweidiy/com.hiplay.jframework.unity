using System;

namespace JFramework.Unity
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class ViewSceneAttribute : Attribute
    {
        public Type SceneStateType { get; }

        public ViewSceneAttribute(Type sceneStateType)
        {
            SceneStateType = sceneStateType;
        }
    }
}