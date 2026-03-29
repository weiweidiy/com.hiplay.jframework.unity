using System;

namespace JFramework
{
    //to do: 移除
    public class JBytesconverter : IDeserializer
    {
        //byte[] IConverter<byte[]>.Convert(byte[] bytes)
        //{
        //    return bytes;
        //}
        public T ToObject<T>(string str)
        {
            throw new NotImplementedException();
        }

        public T ToObject<T>(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public object ToObject(string str, Type type)
        {
            throw new NotImplementedException();
        }

        public object ToObject(byte[] bytes, Type type)
        {
            throw new NotImplementedException();
        }
    }
}
