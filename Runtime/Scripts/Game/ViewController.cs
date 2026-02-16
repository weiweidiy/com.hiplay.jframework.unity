using UnityEngine;

///游戏可以服用
namespace JFramework.Unity
{
    public class ViewControllerBaseOpenArgs
    {
        //通用参数
        public string prefabName; //面板名字
    }

    /// <summary>
    /// 视图控制器：负责处理视图事件，更新UI状态，和场景状态机解耦，允许在不同的场景状态中复用同一个视图控制器
    /// </summary>
    public abstract class ViewController 
    {
        public string Name => this.GetType().Name;

        GameContext context;

        /// <summary>
        /// 在state onenter中被调用，注册事件监听
        /// </summary>
        public virtual void Start(GameContext context)
        {
            this.context = context;
            Debug.Log("ViewController Start " + this.GetType());
        }

        public virtual void Stop()
        {
            this.context = null;
        }

        /// <summary>
        /// 打开视图
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="args"></param>
        public abstract void Open<TArg>(TArg args) where TArg : ViewControllerBaseOpenArgs;

        public abstract void Close();

        public abstract void Refresh<TArg>(TArg args) where TArg : ViewControllerBaseOpenArgs;

        //EventManager GetEventManager() => context.Facade.GetEventManager();
    }
}
