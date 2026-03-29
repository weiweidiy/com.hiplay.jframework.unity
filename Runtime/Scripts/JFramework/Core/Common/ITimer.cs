
using System;

namespace JFramework
{
    public interface ITimer
    {
        void Stop();
    }

    public interface ITimerUtils
    {
        ITimer Regist(float interval, int loopTimes, Action action, bool immediatly = false, bool useRealTime = false);

        void Update();
    }

}
