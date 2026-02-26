using System;
using System.Text;
using Newtonsoft.Json;

namespace JFramework.Unity
{
    /// <summary>
    /// 负责数据的序列化和反序列化
    /// </summary>
    public class DefaultDataConverter : IDataConverter
    {
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T ToObject<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        public T ToObject<T>(byte[] bytes)
        {
            var json = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public object ToObject(string str, Type type)
        {
            return JsonConvert.DeserializeObject(str, type);
        }

        public object ToObject(byte[] bytes, Type type)
        {
            var json = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject(json, type);
        }
    }

}

