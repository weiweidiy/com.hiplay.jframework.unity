using System;
using System.Collections.Generic;
using System.Text;

namespace JFramework
{
    /// <summary>
    /// 链式访问数据接口 json, xml等
    /// </summary>
    public interface IChainData
    {
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IChainData this[object key] { get; }

        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="queryCommand"></param>
        /// <returns></returns>
        IEnumerable<IChainData> SelectMany(string queryCommand);

        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="queryCommand"></param>
        /// <returns></returns>
        IChainData Select(string queryCommand);

        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="queryFunc"></param>
        /// <returns></returns>
        //IEnumerable<IChainData> Select(Func<IChainData, bool> queryFunc);

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">字段名称</param>
        /// <returns></returns>
        object GetValue(object key);

        T GetValue<T>(object key);

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        object GetValue();

        T GetValue<T>();

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="value"></param>
        void SetValue(object value);

        /// <summary>
        /// 添加一个键值对数据对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void AddObject(string key, object value);

        /// <summary>
        /// 删除一个指定名称对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool RemoveObject(string key);

        /// <summary>
        /// 删除自身节点
        /// </summary>
        void RemoveObject();

        /// <summary>
        /// 把对应的JToken转成C#对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <returns></returns>
        T ToObject<T>(object serializer = null);
    }
}
