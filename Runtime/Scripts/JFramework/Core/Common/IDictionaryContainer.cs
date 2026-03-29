using System;
using System.Collections.Generic;



namespace JFramework
{
    /// <summary>
    /// 容器接口，更
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDictionaryContainer<T>
    {
        event Action<ICollection<T>> onItemAdded;

        event Action<T> onItemRemoved;

        event Action<T> onItemUpdated;

        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="teamMember"></param>
        void Add(T member);

        /// <summary>
        /// 添加多個成員
        /// </summary>
        /// <param name="collection"></param>
        void AddRange(IEnumerable<T> collection);

        /// <summary>
        /// 删除成员
        /// </summary>
        /// <param name="uid"></param>
        bool Remove(string uid);

        /// <summary>
        /// 更新成员
        /// </summary>
        /// <param name="teamMember"></param>
        void Update(T member);

        /// <summary>
        /// 获取指定id成员
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        T Get(string uid);

        /// <summary>
        /// 获取方法
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        bool TryGet(string uid, out T item);

        /// <summary>
        /// 根据条件获取
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<T> Get(Predicate<T> predicate);

        /// <summary>
        /// 获取所有成员
        /// </summary>
        /// <returns></returns>
        List<T> GetAll();

        /// <summary>
        /// 长度
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// 清理
        /// </summary>
        void Clear();
    }
}

