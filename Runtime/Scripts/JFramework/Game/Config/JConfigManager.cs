using JFramework.Game;
using JFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JFramework.Game
{
    public class JConfigManager : IJConfigManager, IDisposable
    {
        private string basePath = "";
        private string extend = "";
        private readonly Dictionary<Type, object> _tables = new Dictionary<Type, object>();
        private readonly IConfigLoader loader;
        private readonly Dictionary<Type, Dictionary<string, IUnique>> _uidMaps = new Dictionary<Type, Dictionary<string, IUnique>>();
        private readonly Dictionary<Type, TableInfo> _registrations = new Dictionary<Type, TableInfo>();
        private readonly object _lock = new object();

        public class TableInfo
        {
            public string Path;
            public Type TableType;
            public Type ItemType;
            public IDeserializer Deserializer;
        }

        public JConfigManager(IConfigLoader loader)
        {
            this.loader = loader;
        }

        public void RegisterTable<TTable, TItem>(string path, IDeserializer deserializer)
            where TTable : IConfigTable<TItem>, new()
            where TItem : IUnique
        {
            lock (_lock)
            {
                _registrations[typeof(TItem)] = new TableInfo
                {
                    Path = path,
                    TableType = typeof(TTable),
                    ItemType = typeof(TItem),
                    Deserializer = deserializer
                };
                _uidMaps[typeof(TItem)] = new Dictionary<string, IUnique>();
            }
        }

        public async Task PreloadAllAsync(string basePath = "", string extend = "", IProgress<LoadProgress> progress = null)
        {
            lock (_lock)
            {
                this.basePath = basePath;
                this.extend = extend;
            }
            TableInfo[] tables;
            lock (_lock)
            {
                tables = _registrations.Values.ToArray();
            }
            int totalCount = tables.Length;
            int completed = 0;

            var loadTasks = tables.Select(async tableInfo =>
            {
                try
                {
                    await LoadTableAsync(tableInfo, tableInfo.Deserializer);

                    lock (_lock)
                    {
                        completed++;
                        progress?.Report(new LoadProgress
                        {
                            Current = completed,
                            Total = totalCount,
                            CurrentTable = tableInfo.ItemType.Name,
                            IsDone = completed == totalCount
                        });
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            });

            await Task.WhenAll(loadTasks);
        }

        private async Task LoadTableAsync(TableInfo tableInfo, IDeserializer deserializer)
        {
            object table = Activator.CreateInstance(tableInfo.TableType);

            string path;
            string ext;
            lock (_lock)
            {
                path = basePath;
                ext = extend;
            }
            var data = await loader.LoadBytesAsync(path + tableInfo.Path + ext);

            object itemList = null;
            try
            {
                itemList = deserializer.ToObject(data, tableInfo.ItemType.MakeArrayType());
            }
            catch (Exception)
            {
                throw new Exception($"反序列化表{tableInfo.TableType.ToString()} 失败");
            }

            var initializeMethod = tableInfo.TableType.GetMethod("Initialize");
            initializeMethod.Invoke(table, new object[] { itemList });

            lock (_lock)
            {
                _tables[tableInfo.ItemType] = table;
                var uidMap = _uidMaps[tableInfo.ItemType];
                uidMap.Clear();

                var enumerable = (IEnumerable)table;
                foreach (var item in enumerable)
                {
                    if (item is IUnique uniqueItem && !string.IsNullOrEmpty(uniqueItem.Uid))
                    {
                        uidMap[uniqueItem.Uid] = uniqueItem;
                    }
                }
            }
        }

        public TItem Get<TItem>(string uid) where TItem : class, IUnique
        {
            lock (_lock)
            {
                if (_uidMaps.TryGetValue(typeof(TItem), out var map) &&
                    map.TryGetValue(uid, out var item))
                {
                    return (TItem)item;
                }
            }
            throw new KeyNotFoundException($"{typeof(TItem)}[{uid}] not found");
        }

        public List<TItem> GetAll<TItem>() where TItem : class, IUnique
        {
            lock (_lock)
            {
                if (_tables.TryGetValue(typeof(TItem), out var table))
                {
                    return ((IConfigTable<TItem>)table).ToList();
                }
            }
            return new List<TItem>();
        }

        public List<TItem> Get<TItem>(Func<TItem, bool> predicate) where TItem : class, IUnique
        {
            IConfigTable<TItem> table = null;
            lock (_lock)
            {
                if (!_tables.TryGetValue(typeof(TItem), out var tableObj))
                {
                    return new List<TItem>();
                }
                if (!(tableObj is IConfigTable<TItem> t))
                {
                    throw new InvalidCastException($"Table type mismatch for {typeof(TItem)}");
                }
                table = t;
            }
            try
            {
                return predicate != null
                    ? table.Where(predicate).ToList()
                    : table.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Predicate evaluation failed", ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (_lock)
                {
                    foreach (var table in _tables.Values.OfType<IDisposable>())
                    {
                        table.Dispose();
                    }
                    _tables.Clear();
                    _uidMaps.Clear();
                    _registrations.Clear();

                    if (loader is IDisposable disposableLoader)
                    {
                        disposableLoader.Dispose();
                    }
                }
            }
        }

        ~JConfigManager()
        {
            Dispose(false);
        }
    }

    public struct LoadProgress
    {
        /// <summary>当前已加载表数量</summary>
        public int Current { get; set; }

        /// <summary>总表数量</summary>
        public int Total { get; set; }

        /// <summary>当前正在加载的表名</summary>
        public string CurrentTable { get; set; }

        /// <summary>是否全部加载完成</summary>
        public bool IsDone { get; set; }

        /// <summary>计算加载进度百分比</summary>
        public float Progress => Total > 0 ? (float)Current / Total : 0f;
    }
}

//public class JConfigManager : IJConfigManager, IDisposable
//{
//    string basePath = "";
//    string extend = "";
//    private readonly Dictionary<Type, object> _tables = new Dictionary<Type, object>();
//    private readonly IConfigLoader loader;
//    private readonly Dictionary<Type, Dictionary<string, IUnique>> _uidMaps = new Dictionary<Type, Dictionary<string, IUnique>>();
//    private readonly Dictionary<Type, TableInfo> _registrations = new Dictionary<Type, TableInfo>();

//    public class TableInfo
//    {
//        public string Path;
//        public Type TableType;
//        public Type ItemType;
//        public IDeserializer Deserializer;
//    }

//    public JConfigManager(IConfigLoader loader)
//    {
//        this.loader = loader;
//    }


//    /// <summary>
//    /// 注册配置表
//    /// </summary>
//    /// <typeparam name="TTable"></typeparam>
//    /// <typeparam name="TItem"></typeparam>
//    /// <param name="path"></param>
//    public void RegisterTable<TTable, TItem>(string path, IDeserializer deserializer)
//        where TTable : IConfigTable<TItem>, new()
//        where TItem : IUnique
//    {
//        _registrations[typeof(TItem)] = new TableInfo
//        {
//            Path = path,
//            TableType = typeof(TTable),
//            ItemType = typeof(TItem),
//            Deserializer = deserializer

//        };

//        // 预初始化UID映射
//        _uidMaps[typeof(TItem)] = new Dictionary<string, IUnique>();
//    }

//    /// <summary>
//    /// 预加载所有注册的配置表
//    /// </summary>
//    public async Task PreloadAllAsync(string basePath = "" , string extend = "" , IProgress<LoadProgress> progress = null)
//    {
//        this.basePath = basePath;
//        this.extend = extend;
//        var tables = _registrations.Values.ToArray();
//        int totalCount = tables.Length;
//        int completed = 0;

//        var loadTasks = tables.Select(async tableInfo =>
//        {
//            try
//            {
//                await LoadTableAsync(tableInfo, tableInfo.Deserializer);

//                // 更新进度
//                completed++;
//                progress?.Report(new LoadProgress
//                {
//                    Current = completed,
//                    Total = totalCount,
//                    CurrentTable = tableInfo.ItemType.Name,
//                    IsDone = completed == totalCount
//                });
//            }
//            catch (Exception ex)
//            {
//                // 可以在这里添加错误处理
//                throw;
//            }
//        });

//        await Task.WhenAll(loadTasks);
//    }

//    /// <summary>
//    /// 加载单个配置表
//    /// </summary>
//    private async Task LoadTableAsync(TableInfo tableInfo, IDeserializer deserializer)
//    {
//        // 1. 创建表实例（无需强制转换）
//        var table = Activator.CreateInstance(tableInfo.TableType);

//        // 2. 加载原始数据
//        var data = await loader.LoadBytesAsync(basePath + tableInfo.Path + extend);

//        object itemList = null;
//        try
//        {
//            itemList = deserializer.ToObject(data, tableInfo.ItemType.MakeArrayType());
//        }
//        catch(Exception e)
//        {
//            throw new Exception($"反序列化表{tableInfo.TableType.ToString()} 失败");
//        }

//        //to do: 序列化成

//        // 3. 通过反射调用Initialize方法
//        var initializeMethod = tableInfo.TableType.GetMethod("Initialize");
//        initializeMethod.Invoke(table, new object[] { itemList });

//        // 4. 存储表引用
//        _tables[tableInfo.ItemType] = table;

//        // 5. 构建UID索引
//        var uidMap = _uidMaps[tableInfo.ItemType];
//        uidMap.Clear();

//        // 通过IEnumerable接口遍历（非泛型方式）
//        var enumerable = (IEnumerable)table;
//        foreach (var item in enumerable)
//        {
//            if (item is IUnique uniqueItem && !string.IsNullOrEmpty(uniqueItem.Uid))
//            {
//                uidMap[uniqueItem.Uid] = uniqueItem;
//            }
//        }
//    }

//    /// <summary>
//    /// 获取指定uid对象
//    /// </summary>
//    /// <typeparam name="TItem"></typeparam>
//    /// <param name="uid"></param>
//    /// <returns></returns>
//    /// <exception cref="KeyNotFoundException"></exception>
//    public TItem Get<TItem>(string uid) where TItem : class, IUnique
//    {
//        if (_uidMaps.TryGetValue(typeof(TItem), out var map) &&
//            map.TryGetValue(uid, out var item))
//        {
//            return (TItem)item;
//        }
//        throw new KeyNotFoundException($"{typeof(TItem)}[{uid}] not found");
//    }


//    /// <summary>
//    /// 获取所有对象
//    /// </summary>
//    /// <typeparam name="TItem"></typeparam>
//    /// <returns></returns>
//    public List<TItem> GetAll<TItem>() where TItem : class, IUnique
//    {
//        if (_tables.TryGetValue(typeof(TItem), out var table))
//        {
//            return ((IConfigTable<TItem>)table).ToList();
//        }
//        return new List<TItem>();
//    }

//    /// <summary>
//    /// 根据条件获取
//    /// </summary>
//    /// <typeparam name="TItem"></typeparam>
//    /// <param name="predicate"></param>
//    /// <returns></returns>
//    /// <exception cref="InvalidCastException"></exception>
//    /// <exception cref="InvalidOperationException"></exception>
//    public List<TItem> Get<TItem>(Func<TItem, bool> predicate) where TItem : class, IUnique
//    {
//        // 1. 尝试获取已加载的表
//        if (!_tables.TryGetValue(typeof(TItem), out var tableObj))
//        {
//            return new List<TItem>();
//        }

//        // 2. 类型安全转换
//        if (!(tableObj is IConfigTable<TItem> table))
//        {
//            throw new InvalidCastException($"Table type mismatch for {typeof(TItem)}");
//        }

//        // 3. 应用筛选条件
//        try
//        {
//            return predicate != null
//                ? table.Where(predicate).ToList()
//                : table.ToList();
//        }
//        catch (Exception ex)
//        {
//            throw new InvalidOperationException("Predicate evaluation failed", ex);
//        }
//    }

//    public void Dispose()
//    {
//        Dispose(true);
//        GC.SuppressFinalize(this);
//    }

//    /// <summary>
//    /// 释放资源
//    /// </summary>
//    /// <param name="disposing"></param>
//    protected virtual void Dispose(bool disposing)
//    {
//        if (disposing)
//        {
//            // 释放托管资源
//            foreach (var table in _tables.Values.OfType<IDisposable>())
//            {
//                table.Dispose();
//            }

//            _tables.Clear();
//            _uidMaps.Clear();
//            _registrations.Clear();

//            if (loader is IDisposable disposableLoader)
//            {
//                disposableLoader.Dispose();
//            }
//        }

//        // 这里可以释放非托管资源（如果有）
//        // 当前示例中没有非托管资源需要释放
//    }

//    // 可选：添加终结器以防忘记调用Dispose
//    ~JConfigManager()
//    {
//        Dispose(false);
//    }
//}
