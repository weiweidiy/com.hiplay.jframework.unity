using System;
using System.Collections.Generic;
using JFramework;

namespace JFramework.Unity
{
    public sealed class EventBusAdapter : IEventBus
    {
        private readonly EventManager eventManager;
        private readonly Dictionary<Delegate, Delegate> handlerMap = new();

        public EventBusAdapter(EventManager eventManager)
        {
            this.eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
        }

        public void Subscribe<TEvent>(Action<TEvent> handler, bool forceHandle = false) where TEvent : Event
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            if (handlerMap.ContainsKey(handler))
                return;

            EventManager.EventDelegate<TEvent> wrapper = evt => handler(evt);
            handlerMap[handler] = wrapper;
            eventManager.AddListener(wrapper, forceHandle);
        }

        public void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : Event
        {
            if (handler == null)
                return;

            if (handlerMap.TryGetValue(handler, out var wrapper))
            {
                eventManager.RemoveListener((EventManager.EventDelegate<TEvent>)wrapper);
                handlerMap.Remove(handler);
            }
        }

        public void Publish<TEvent>(TEvent evt) where TEvent : Event
        {
            if (evt == null)
                throw new ArgumentNullException(nameof(evt));

            eventManager.Raise(evt);
        }

        public void Publish<TEvent>(object arg) where TEvent : Event, new()
        {
            eventManager.Raise<TEvent>(arg);
        }
    }
}
