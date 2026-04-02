using System;
using System.Collections.Generic;

namespace JFramework.Unity
{
    public sealed class SceneStateRegistry : ISceneStateRegistry
    {
        private readonly Dictionary<Type, ISceneState> states = new();

        public void Register<TState>(TState state) where TState : class, ISceneState
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            states[typeof(TState)] = state;
        }

        public TState Get<TState>() where TState : class, ISceneState
        {
            return Get(typeof(TState)) as TState;
        }

        public ISceneState Get(Type stateType)
        {
            if (stateType == null)
                throw new ArgumentNullException(nameof(stateType));

            if (!states.TryGetValue(stateType, out var state))
                throw new InvalidOperationException($"Scene state not found: {stateType.FullName}");

            return state;
        }

        public bool TryGet<TState>(out TState state) where TState : class, ISceneState
        {
            if (states.TryGetValue(typeof(TState), out var raw))
            {
                state = raw as TState;
                return state != null;
            }

            state = null;
            return false;
        }
    }
}
