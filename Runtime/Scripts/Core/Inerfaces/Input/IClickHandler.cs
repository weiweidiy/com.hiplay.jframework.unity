using System;

namespace JFramework.Unity
{
    public interface IClickHandler
    {
        event Action<object> onClicked;
        object TargetArg { get; set; }
    }
}


