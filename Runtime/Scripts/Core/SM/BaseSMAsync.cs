
using Cysharp.Threading.Tasks;
using Stateless;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace JFramework.Unity
{
    /// <summary>
    /// 游戏状态机-> Login , Game 等等 
    /// </summary>
    public abstract class BaseSMAsync<TState, TTrigger> : ISceneStateMachineAsync where TState : BaseStateAsync 
    {

        public class SMConfig
        {
            public TState state;
            public Dictionary<TTrigger, TState> dicPermit;
            public StateMachine<TState, TTrigger>.TriggerWithParameters<object> parameter;
        }

        //[Inject]
        //protected IInjectionContainer container;

        /// <summary>
        /// 状态机
        /// </summary>
        protected StateMachine<TState, TTrigger> machine;

        /// <summary>
        /// 
        /// </summary>
        protected GameContext context;

        /// <summary>
        /// 所有状态列表
        /// </summary>
        List<TState> states;


        public BaseSMAsync()
        {
            Initialize();
        }

        /// <summary>
        /// 初始化状态机
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="Exception"></exception>
        void Initialize()
        {

            //从子类获取所有状态实例
            states = GetAllStates();

            if (states == null || states.Count == 0)
                throw new Exception("状态机状态列表为空 " + GetType().ToString());

            Debug.Log("before StateMachine create ");

            machine = new StateMachine<TState, TTrigger>(states[0]);

            Debug.Log("StateMachine success " + machine.ToString());

            //获取所有状态配置
            var configs = GetConfigs();
            var keys = configs.Keys;
            foreach (var key in keys)
            {
                var config = configs[key];
                var state = config.state;

                //配置状态
                var cfg = machine.Configure(state);

                //配置状态机的参数
                if (config.parameter != null) //如果有参数
                    cfg = cfg.OnEntryFromAsync(config.parameter, async (arg) => await OnEnter(state, arg));
                else
                    cfg = cfg.OnEntryAsync(async () => { await OnEnter(state); });

                cfg = cfg.OnExitAsync(async () => { await OnExit(state); });


                var triggerKeys = config.dicPermit.Keys;
                foreach (var triggerKey in triggerKeys)
                {
                    var targetState = config.dicPermit[triggerKey];
                    cfg = cfg.Permit(triggerKey, targetState);
                }
            }
        }


        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private async UniTask OnEnter(TState state, object arg = null)
        {
            await state.OnEnter(context, arg);
        }

        /// <summary>
        /// 离开状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private async UniTask OnExit(TState state)
        {
            await state.OnExit();
        }

        /// <summary>
        /// 获取所有状态配置
        /// </summary>
        /// <returns></returns>
        protected abstract Dictionary<string, SMConfig> GetConfigs();

        /// <summary>
        /// 获取所有状态
        /// </summary>
        /// <returns></returns>
        protected abstract List<TState> GetAllStates();

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="stateName"></param>
        public abstract Task SwitchToState(string stateName);

        /// <summary>
        /// 设置游戏上下文，状态机和状态都可以通过上下文获取游戏的各种服务
        /// </summary>
        /// <param name="context"></param>
        public void SetContext(GameContext context) => this.context = context;

    }
}
