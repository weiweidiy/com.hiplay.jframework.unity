using System;
using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// 通用启动器，负责启动应用
    /// </summary>
    public class CommonLauncher : BaseRunable
    {
        /// <summary>
        /// 内部运行对象
        /// </summary>
        protected Queue<IRunable> internalRunables;


        public CommonLauncher() { }

        public CommonLauncher(Queue<IRunable> runables)
        {
            this.internalRunables = runables;
        }

        /// <summary>
        /// 运行
        /// </summary>
        protected override void OnStart(RunableExtraData extraData = null)
        {
            base.OnStart(extraData);

            RunNext(extraData);
        }

        /// <summary>
        /// 运行下一个可运行对象
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void RunNext(RunableExtraData extraData)
        {
            if (internalRunables == null)
                throw new NullReferenceException("runables is null");

            if(internalRunables.Count == 0)
            {
                Stop();
                return;
            }

            var runable = internalRunables.Dequeue();
            runable.onComplete += InternalRunable_onComplte;
            runable.Start(extraData);
        }

        /// <summary>
        /// 内部运行对象运行完毕
        /// </summary>
        /// <param name="runable"></param>
        private void InternalRunable_onComplte(IRunable runable)
        {
            RunNext(runable.ExtraData);
        }


    }
}

