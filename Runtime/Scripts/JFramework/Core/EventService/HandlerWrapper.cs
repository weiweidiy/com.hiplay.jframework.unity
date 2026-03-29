using System;


namespace JFramework
{

    /// <summary>
    /// handler实例的容器：
    /// </summary>
    public struct HandlerWrapper
    {

        internal HandlerWrapper(EventDelegate handler, bool forceHandle)
        {
            _handler = handler;
            _forceHandle = forceHandle;
        }


        public Delegate Handler
        {
            get { return _handler; }
        }

        public bool ForceHandle
        {
            get { return _forceHandle; }
        }

        internal void InvokeHandler(Event e)
        {
            if ((e.Handled == false) || (_forceHandle == true))
            {
                _handler.Method.Invoke(_handler.Target, new object[] { e });
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is HandlerWrapper))
                return false;

            return Equals((HandlerWrapper)obj);
        }

        public bool Equals(HandlerWrapper handlerInfo)
        {
            return _handler == handlerInfo._handler && _forceHandle == handlerInfo._forceHandle;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public static bool operator ==(HandlerWrapper handlerInfo1, HandlerWrapper handlerInfo2)
        {
            return handlerInfo1.Equals(handlerInfo2);
        }

        public static bool operator !=(HandlerWrapper handlerInfo1, HandlerWrapper handlerInfo2)
        {
            return !handlerInfo1.Equals(handlerInfo2);
        }


        private Delegate _handler;
        private bool _forceHandle;


    }
}