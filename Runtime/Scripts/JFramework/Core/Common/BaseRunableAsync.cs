using System;
using System.Threading.Tasks;

namespace JFramework
{
    public abstract class BaseRunableAsync : BaseRunable
    {
        public override async Task Start(RunableExtraData extraData, TaskCompletionSource<bool> tcs = null)
        {
            if (IsRunning)
            {
                throw new Exception(this.GetType().ToString() + " is running , can't run again! ");
            }

            this.tcs = tcs == null ? new TaskCompletionSource<bool>() : tcs;

            this.ExtraData = extraData;
            this.IsRunning = true;

            await OnStartAync(extraData);
            await this.tcs.Task;
        }

        protected virtual Task OnStartAync(RunableExtraData extraData)
        {
            return Task.CompletedTask;
        }
    }
}

