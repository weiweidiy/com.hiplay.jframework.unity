using Cysharp.Threading.Tasks;

namespace JFramework.Unity
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseStateAsync<TContext>
    {
        /// <summary>
        /// 上下文
        /// </summary>
        protected TContext context;

        /// <summary>
        /// 状态机名字
        /// </summary>
        public string Name => GetType().Name;

        /// <summary>
        /// 状态进入时调用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual UniTask OnEnter(TContext context, object arg)
        {
            this.context = context;
            AddListeners();      
            return OnEnter(arg); 
        }

        /// <summary>
        /// 子类实现
        /// </summary>
        /// <returns></returns>
        protected virtual UniTask OnEnter(object arg) => UniTask.CompletedTask;

        /// <summary>
        /// 状态退出时调用
        /// </summary>
        /// <returns></returns>
        public virtual UniTask OnExit()
        {
            RemoveListeners();
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// 事件监听器，在状态进入时调用，子类重写
        /// </summary>
        public virtual void AddListeners() { }
        public virtual void RemoveListeners() { }
    }
}
