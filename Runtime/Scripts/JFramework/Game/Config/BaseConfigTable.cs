using System.Collections;
using System.Collections.Generic;

namespace JFramework.Game
{
    /// <summary>
    /// 表对象，一般用在自动生成的配置表父类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseConfigTable<T> : IConfigTable<T>, IEnumerable<T> where T : IUnique
    {
        private readonly List<T> _items = new List<T>();

        public void Initialize(T[] lst) => _items.AddRange(lst);

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
