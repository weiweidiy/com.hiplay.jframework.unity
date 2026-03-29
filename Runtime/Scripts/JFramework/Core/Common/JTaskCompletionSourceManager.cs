using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace JFramework
{
    /// <summary>
    /// taskCompletionSource管理器，负责管理所有未完成的任务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JTaskCompletionSourceManager<T> : IJTaskCompletionSourceManager<T>
    {


        ConcurrentDictionary<string, TaskCompletionSource<T>> pendingResponses = new ConcurrentDictionary<string, TaskCompletionSource<T>>();

        public TaskCompletionSource<T> AddTask(string uid)
        {
            var tcs = new TaskCompletionSource<T>();
            if (!pendingResponses.TryAdd(uid, tcs)) // 使用 TryAdd 避免冲突
                return null;

            return tcs;
        }

        /// <summary>
        /// 移除一个任务
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool RemoveTask(string uid)
        {
            return pendingResponses.TryRemove(uid, out _);
        }

        /// <summary>
        /// 获取缓存中的任务
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public TaskCompletionSource<T> GetTask(string uid)
        {
            TaskCompletionSource<T> result = null;
            pendingResponses.TryGetValue(uid, out result);
            return result;
        }

        /// <summary>
        /// 设置任务结果
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="result"></param>
        public void SetResult(string uid, T result)
        {
            var task = GetTask(uid);
            task.SetResult(result);
        }

        /// <summary>
        /// 设置任务异常
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="exception"></param>
        public void SetException(string uid, Exception exception)
        {
            var task = GetTask(uid);
            task.SetException(exception);
        }

        /// <summary>
        /// 等待任务
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        /// <exception cref="TimeoutException"></exception>
        public async Task<T> WaitingTask(string uid, TimeSpan? timeout = null)
        {
            var task = GetTask(uid);
            var timeoutTask = Task.Delay(timeout ?? TimeSpan.FromSeconds(10));
            var completedTask = await Task.WhenAny(task.Task, timeoutTask);
            if (completedTask == timeoutTask)
                throw new TimeoutException("Request timed out.");

            return await task.Task; // 等待直到 OnWebSocketMessage 调用 TrySetResult
        }

    }
}
