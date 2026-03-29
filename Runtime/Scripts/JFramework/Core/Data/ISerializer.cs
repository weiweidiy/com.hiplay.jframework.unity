using System;

namespace JFramework
{
    public interface ISerializer
    {
        string Serialize(object obj);
    }

    public interface IDeserializer
    {
        T ToObject<T>(string str);

        T ToObject<T>(byte[] bytes);

        object ToObject(string str, Type type);

        object ToObject(byte[] bytes, Type type);
    }
}
