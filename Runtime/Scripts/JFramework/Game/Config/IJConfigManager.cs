using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JFramework.Game
{
    public interface IJConfigManager
    {
        List<TItem> Get<TItem>(Func<TItem, bool> predicate) where TItem : class, IUnique;
        TItem Get<TItem>(string uid) where TItem : class, IUnique;
        List<TItem> GetAll<TItem>() where TItem : class, IUnique;
        Task PreloadAllAsync(string basePath = "", string extend = "", IProgress<LoadProgress> progress = null);
        void RegisterTable<TTable, TItem>(string path, IDeserializer deserializer)
            where TTable : IConfigTable<TItem>, new()
            where TItem : IUnique;
    }
}