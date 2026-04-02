using System;
using JFramework;

namespace JFramework.Unity
{
    public interface IEventBus
    {
        void Subscribe<TEvent>(Action<TEvent> handler, bool forceHandle = false) where TEvent : Event;

        void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : Event;

        void Publish<TEvent>(TEvent evt) where TEvent : Event;

        void Publish<TEvent>(object arg) where TEvent : Event, new();
    }
}
