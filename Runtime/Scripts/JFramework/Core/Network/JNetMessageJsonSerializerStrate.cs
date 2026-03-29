using System;
using System.Text;

namespace JFramework
{
    /// <summary>
    /// 使用json序列化反序列化的策略
    /// </summary>
    public class JNetMessageJsonSerializerStrate : INetMessageSerializerStrate
    {
        IDataConverter dataConverter;
        public JNetMessageJsonSerializerStrate(IDataConverter dataConverter)
        {
            this.dataConverter = dataConverter;
        }

        public byte[] Serialize(IJNetMessage obj)
        {
            return Encoding.UTF8.GetBytes(dataConverter.Serialize(obj));
        }

        public IJNetMessage Deserialize(byte[] data, IMessageTypeResolver typeResolver)
        {
            //处理接收的数据=>反序列化等()
            string json;
            try
            {
                json = Encoding.UTF8.GetString(data);
            }
            catch (DecoderFallbackException)
            {
                throw new Exception("Invalid UTF-8 data received.");
            }

            var messageType = typeResolver.ResolveMessageType(data);
            return (IJNetMessage)dataConverter.ToObject(json, messageType);
        }
    }
}