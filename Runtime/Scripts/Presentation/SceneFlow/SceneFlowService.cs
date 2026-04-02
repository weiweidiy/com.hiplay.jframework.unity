using System;
using Cysharp.Threading.Tasks;

namespace JFramework.Unity
{
    public sealed class SceneFlowService : ISceneFlow
    {
        private readonly ISceneStateRegistry stateRegistry;
        private readonly ISceneContext context;
        private readonly Type firstStateType;
        private ISceneState currentState;

        public SceneFlowService(ISceneStateRegistry stateRegistry, ISceneContext context, Type firstStateType)
        {
            this.stateRegistry = stateRegistry ?? throw new ArgumentNullException(nameof(stateRegistry));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.firstStateType = firstStateType ?? throw new ArgumentNullException(nameof(firstStateType));
        }

        public async UniTask EnterFirstAsync()
        {
            await EnterAsync(firstStateType, null);
        }

        public async UniTask EnterAsync<TState>(object arg = null) where TState : class, ISceneState
        {
            await EnterAsync(typeof(TState), arg);
        }

        private async UniTask EnterAsync(Type stateType, object arg)
        {
            if (currentState != null)
            {
                await currentState.ExitAsync();
            }

            currentState = stateRegistry.Get(stateType);
            await currentState.EnterAsync(context, arg);
        }
    }
}
