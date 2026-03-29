using System;
using System.Threading.Tasks;

namespace JFramework
{
    public interface IJTaskCompletionSourceManager<T>
    {
        TaskCompletionSource<T> AddTask(string uid);
        TaskCompletionSource<T> GetTask(string uid);
        bool RemoveTask(string uid);
        void SetException(string uid, Exception exception);
        void SetResult(string uid, T result);
        Task<T> WaitingTask(string uid, TimeSpan? timeout = null);
    }
}