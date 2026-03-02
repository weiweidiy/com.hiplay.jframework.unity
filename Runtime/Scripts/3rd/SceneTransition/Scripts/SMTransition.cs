

using Cysharp.Threading.Tasks;
using JFramework;
using System;
using System.Collections;
using UnityEngine;

namespace JFramework.Unity
{
    /// <summary>
    /// The base class for all level transitions.
    /// </summary>
    public abstract class SMTransition : MonoBehaviour, ITransition
    {
        protected virtual void Awake()
        {
            // Ensure the transition object is not destroyed on scene load
            //DontDestroyOnLoad(gameObject);
        }

        protected SMTransitionState state = SMTransitionState.Out;

        /// <summary>
        /// 离开当前效果
        /// </summary>
        /// <param name="complete"></param>
        public virtual void TransitionOut(Action complete)
        {
            StartCoroutine(DoTransitionOut(complete));
        }

        public virtual async UniTask<SMTransitionState> TransitionOut()
        {
            state = SMTransitionState.Out;
            Prepare();
            float time = 0;

            while (Process(time))
            {
                time += Time.deltaTime;
                // wait for the next frame
                await UniTask.DelayFrame(1);
                //yield return 0;
            }

            // wait another frame...
            await UniTask.DelayFrame(1);
            //yield return 0;

            state = SMTransitionState.Hold;
            DontDestroyOnLoad(gameObject);

            // wait another frame...
            //yield return 0;
            return state;
        }

        /// <summary>
        /// 进入当前效果
        /// </summary>
        /// <param name="complete"></param>
        public virtual void TransitionIn(Action complete)
        {
            StartCoroutine(DoTransitionIn(complete));
        }

        public virtual async UniTask<SMTransitionState> TransitionIn()
        {
            // wait another frame...
            await UniTask.DelayFrame(1);

            state = SMTransitionState.In;
            Prepare();
            float time = 0;

            while (Process(time))
            {
                time += Time.deltaTime;
                // wait for the next frame
                await UniTask.DelayFrame(1);
            }

            // wait another frame...
            await UniTask.DelayFrame(1);

            Destroy(gameObject);

            return state;
        }

        /// <summary>
        /// This method actually does the transition. It is run in a coroutine and therefore needs to do
        /// yield returns to play an animation or do another progress over time. When this method returns
        /// the transition is expected to be finished.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator"/> for showing the transition status. Use yield return statements to keep
        /// the transition running, otherwise simply end the method to stop the transition.
        /// </returns>
        protected virtual IEnumerator DoTransitionOut(Action complete)
        {
            state = SMTransitionState.Out;
            Prepare();
            float time = 0;

            while (Process(time))
            {
                time += Time.deltaTime;
                // wait for the next frame
                yield return 0;
            }

            // wait another frame...
            yield return 0;

            state = SMTransitionState.Hold;
            DontDestroyOnLoad(gameObject);

            // wait another frame...
            //yield return 0;

            complete?.Invoke();
        }


        /// <summary>
        /// 进入
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator DoTransitionIn(Action complete)
        {
            //IEnumerator loadLevel = DoLoadLevel();
            //while (loadLevel.MoveNext())
            //{
            //	yield return loadLevel.Current;
            //}

            // wait another frame...
            yield return 0;

            state = SMTransitionState.In;
            Prepare();
            float time = 0;

            while (Process(time))
            {
                time += Time.deltaTime;
                // wait for the next frame
                yield return 0;
            }

            // wait another frame...
            yield return 0;

            Destroy(gameObject);

            complete?.Invoke();
        }

        /// <summary>
        /// invoked at the start of the <see cref="SMTransitionState.In"/> and <see cref="SMTransitionState.Out"/> state to 
        /// initialize the transition
        /// </summary>
        protected virtual void Prepare()
        {
        }

        /// <summary>
        /// Invoked once per frame while the transition is in state <see cref="SMTransitionState.In"/> or <see cref="SMTransitionState.Out"/> 
        /// to calculate the progress
        /// </summary>
        /// <param name='elapsedTime'>
        /// the time that has elapsed since the start of current transition state in seconds. 
        /// </param>
        /// <returns>
        /// false if no further calls are necessary for the current state, true otherwise
        /// </returns>
        protected abstract bool Process(float elapsedTime);
    }
}