
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JFramework
{

    /// <summary>
    /// 存档工具（可以理解为保存一张张数据表，可以作为数据库存取接口使用)
    /// </summary>
    public class JDataStore : IGameDataStore, IDisposable
    {
        private readonly Dictionary<string, object> _memoryCache = new Dictionary<string, object>();
        private readonly IDataManager dataManager;
        private bool _disposed;

        public JDataStore(IDataManager dataManager)
        {
            this.dataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
        }

        /// <summary>
        /// 是否存在存档
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> ExistsAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or whitespace", nameof(key));

            // 先检查内存缓存
            if (_memoryCache.ContainsKey(key))
                return true;

            // 然后检查持久化存储
            return await dataManager.ExistsAsync(key);
        }

        /// <summary>
        /// 获取存档数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or whitespace", nameof(key));

            // 先检查内存缓存
            if (_memoryCache.TryGetValue(key, out var cachedValue))
            {
                if (cachedValue is T typedValue)
                    return typedValue;

                // 如果类型不匹配，从存储重新加载
            }

            // 从持久化存储读取
            var data = await dataManager.ReadAsync<T>(key, dataManager);
            if (data == null)
                return default;

            // 更新内存缓存
            _memoryCache[key] = data;

            return data;
        }

        /// <summary>
        /// 存档，持久化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task SaveAsync<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or whitespace", nameof(key));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            // 序列化数据
            var serializedData = dataManager.Serialize(value);

            // 写入持久化存储
            await dataManager.WriteAsync(key, serializedData);

            // 更新内存缓存
            _memoryCache[key] = value;
        }

        /// <summary>
        /// 删除存档
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> RemoveAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or whitespace", nameof(key));

            // 从持久化存储删除
            var success = await dataManager.DeleteAsync(key);

            // 从内存缓存删除
            _memoryCache.Remove(key);

            return success;
        }

        /// <summary>
        /// 删除所有表
        /// </summary>
        /// <returns></returns>
        public async Task ClearAsync()
        {
            // 清空持久化存储
            await dataManager.ClearAsync();

            // 清空内存缓存
            _memoryCache.Clear();
        }

        /// <summary>
        /// 清理缓存数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetAllKeysAsync()
        {
            // 获取所有持久化存储的键
            //var persistentKeys = await _reader.GetAllKeysAsync();

            // 合并内存缓存的键
            var memoryKeys = _memoryCache.Keys;

            // 返回合并后的唯一键集合
            return memoryKeys; // persistentKeys.Union(memoryKeys).Distinct();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {

                _memoryCache.Clear();
            }

            _disposed = true;
        }

        ~JDataStore()
        {
            Dispose(false);
        }
    }
}