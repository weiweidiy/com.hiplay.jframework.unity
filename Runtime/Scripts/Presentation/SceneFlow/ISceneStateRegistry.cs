using System;

namespace JFramework.Unity
{
    public interface ISceneStateRegistry
    {
        void Register<TState>(TState state) where TState : class, ISceneState;

        TState Get<TState>() where TState : class, ISceneState;

        ISceneState Get(Type stateType);

        bool TryGet<TState>(out TState state) where TState : class, ISceneState;
    }
}
