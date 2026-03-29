using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace JFramework
{
    public abstract class JObjectPool : IObjectPool
    {
        // 存储类型对应的委托三元组
        private readonly Dictionary<Type, (Action<object>, Action<object>, Action<object>)> _delegates = new Dictionary<Type, (Action<object>, Action<object>, Action<object>)>();

        // 修改构造函数以接收委托工厂
        public JObjectPool(ITypeRegister typeRegister,
            Func<Type, Action<object>> rentDelegateFactory = null,
            Func<Type, Action<object>> returnDelegateFactory = null,
            Func<Type, Action<object>> releaseDelegateFactory = null)
        {
            var types = typeRegister.GetTypes();

            foreach (var type in types.Values)
            {
                if (!type.IsClass || type.GetConstructor(Type.EmptyTypes) == null)
                    continue;

                // 获取或创建委托
                var onRent = rentDelegateFactory?.Invoke(type);
                var onReturn = returnDelegateFactory?.Invoke(type);
                var onRelease = releaseDelegateFactory?.Invoke(type);

                // 存储委托供后续使用
                _delegates[type] = (onRent, onReturn, onRelease);

                // 动态调用Regist<T>
                var registMethod = typeof(JObjectPool)
                    .GetMethod("Regist", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetGenericMethodDefinition();

                var boundMethod = registMethod.MakeGenericMethod(type);

                // 将Action<object>转换为Action<T>
                var typedOnRent = onRent != null ? CreateTypedDelegateOptimized(type, onRent) : null;
                var typedOnReturn = onReturn != null ? CreateTypedDelegateOptimized(type, onReturn) : null;
                var typedOnRelease = onRelease != null ? CreateTypedDelegateOptimized(type, onRelease) : null;

                boundMethod.Invoke(this, new object[] { typedOnRent, typedOnReturn, typedOnRelease });
            }
        }

        // 辅助方法：将Action<object>转换为Action<T>
        // 表达式树优化版委托转换
        private Delegate CreateTypedDelegateOptimized(Type type, Action<object> untypedDelegate)
        {
            var param = Expression.Parameter(type, "obj");
            var convert = Expression.Convert(param, typeof(object));
            var call = Expression.Call(
                Expression.Constant(untypedDelegate.Target),
                untypedDelegate.Method,
                convert);

            return Expression.Lambda(
                typeof(Action<>).MakeGenericType(type),
                call,
                param).Compile();
        }


        protected abstract void Regist<T>(Action<T> onRent, Action<T> onReturn, Action<T> onRelease) where T : class, new();
        public abstract T Rent<T>(Action<T> onGet = null);
        public abstract void Return<T>(T obj);
    }
}

