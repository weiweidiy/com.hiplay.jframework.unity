using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 默认数据管理器，使用PlayerPrefs进行数据存储，适用于小型数据的存储和读取。
    /// </summary>
    public class UnityPlayerPrefsDataManager : IDataManager
    {
        IDataConverter dataConverter;
        public UnityPlayerPrefsDataManager(IDataConverter dataConverter) { 
            this.dataConverter = dataConverter;
        }


        public Task ClearAsync()
        {
            PlayerPrefs.DeleteAll();
            return Task.CompletedTask;
        }

        public void Delete(string location)
        {
            PlayerPrefs.DeleteKey(location);
        }

        public Task<bool> DeleteAsync(string location)
        {
            Delete(location);
            return Task.FromResult(true);
        }

        public Task<bool> ExistsAsync(string location)
        {
            var result = PlayerPrefs.HasKey(location);
            return Task.FromResult(result);
        }

        public byte[] Read(string location)
        {
            var str = PlayerPrefs.GetString(location);
            return Encoding.UTF8.GetBytes(str);
        }

        public Task<T> ReadAsync<T>(string location, IDeserializer converter)
        {
            var bytes = Read(location);
            var obj = converter.ToObject<T>(bytes);
            return Task.FromResult(obj);
        }

        public string Serialize(object obj)
        {
            return dataConverter.Serialize(obj);
        }

        public T ToObject<T>(string str)
        {
            return dataConverter.ToObject<T>(str);
        }

        public T ToObject<T>(byte[] bytes)
        {
            var str = Encoding.UTF8.GetString(bytes);
            return ToObject<T>(str);
        }

        public object ToObject(string str, Type type)
        {
            return dataConverter.ToObject(str, type);
        }

        public object ToObject(byte[] bytes, Type type)
        {
            var str = Encoding.UTF8.GetString(bytes);
            return ToObject(str,type);
        }

        public void Write(string toPath, byte[] buffer, Encoding encoding = null)
        {
            if(encoding == null) encoding = Encoding.UTF8;
            var str = Encoding.UTF8.GetString(buffer);
            Write(toPath, str, encoding);


        }

        public void Write(string toPath, string buffer, Encoding encoding = null)
        {
            PlayerPrefs.SetString(toPath, buffer);
        }

        public Task WriteAsync(string toPath, byte[] buffer, Encoding encoding = null)
        {
            Write(toPath, buffer, encoding);
            return Task.CompletedTask;
        }

        public Task WriteAsync(string toPath, string buffer, Encoding encoding = null)
        {
            Write(toPath,buffer, encoding);
            return Task.CompletedTask;
        }
    }

}

