using System;

namespace Game.Common
{
    public interface IClickHandler
    {
        event Action<object> onClicked;
        object TargetArg { get; set; }
    }
}


