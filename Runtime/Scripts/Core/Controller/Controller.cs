using System;
using System.Threading.Tasks;

///游戏可以服用
namespace JFramework.Unity
{
    /// <summary>
    /// 基类
    /// </summary>
    public abstract class Controller
    {
        public abstract Task Do(GameContext context);
    }

    /// <summary>
    /// 泛型版本，提供类型安全的参数传递。
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    public abstract class Controller<TArgs> : Controller
    {
        public abstract Task Do(GameContext context, TArgs args);

        public sealed override Task Do(GameContext context)
        {
            throw new ControllerArgumentException(GetType(), typeof(TArgs), 1, 0);
        }
    }

    public sealed class ControllerArgumentException : ArgumentException
    {
        public ControllerArgumentException(Type controllerType, Type argumentType, int expectedCount, int actualCount)
            : base($"Controller '{controllerType.FullName}' 希望 {expectedCount} 参数数量： '{argumentType.FullName}', 但收到的 {actualCount}.")
        {
        }
    }

    public sealed class ControllerArgumentTypeException : ArgumentException
    {
        public ControllerArgumentTypeException(Type controllerType, Type expectedType, Type actualType)
            : base(BuildMessage(controllerType, expectedType, actualType))
        {
        }

        private static string BuildMessage(Type controllerType, Type expectedType, Type actualType)
        {
            var actualTypeName = actualType != null ? actualType.FullName : "null";
            return $"Controller '{controllerType.FullName}' 希望参数类型 '{expectedType.FullName}', 但收到 '{actualTypeName}'.";
        }
    }
}
