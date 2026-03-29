using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JFramework
{
    public interface IReaderAsync
    {
        Task<T> ReadAsync<T>(string location, IDeserializer converter);
        Task<bool> ExistsAsync(string location);
    }
}
