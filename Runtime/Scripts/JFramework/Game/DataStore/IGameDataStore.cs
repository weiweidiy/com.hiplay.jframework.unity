using System.Collections.Generic;
using System.Threading.Tasks;

namespace JFramework
{
    public interface IGameDataStore
    {
        Task<bool> ExistsAsync(string key);
        Task<T> GetAsync<T>(string key);
        Task SaveAsync<T>(string key, T value);
        Task<bool> RemoveAsync(string key);
        Task ClearAsync();
        Task<IEnumerable<string>> GetAllKeysAsync();
    }
}

