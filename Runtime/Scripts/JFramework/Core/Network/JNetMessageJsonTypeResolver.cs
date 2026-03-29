using System;
using System.Collections.Generic;
using System.Text;

namespace JFramework
{
    /// <summary>
    /// 用json格式解析数据的方式获取数据类型
    /// </summary>
    public class JNetMessageJsonTypeResolver : IMessageTypeResolver
    {
        private readonly Dictionary<int, Type> messageTypes = new Dictionary<int, Type>();

        IDeserializer deserializer;

        public JNetMessageJsonTypeResolver(IDeserializer deserializer) : this(deserializer, null) { }
        public JNetMessageJsonTypeResolver(IDeserializer deserializer, ITypeRegister register)
        {
            this.deserializer = deserializer;

            if (register != null)
            {
                var dic = register.GetTypes();
                foreach (var table in dic)
                {
                    RegisterMessageType(table.Key, table.Value);
                }
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public IMessageTypeResolver RegisterMessageType(int messageId, Type messageType)
        {
            messageTypes[messageId] = messageType;
            return this;
        }

        public Type ResolveMessageType(byte[] data)
        {
            // 假设前4字节是消息ID
            //var messageId = BitConverter.ToInt32(data, 0);
            var messageId = GetMessageTypeId(data);
            if (messageTypes.TryGetValue(messageId, out var type))
            {
                return type;
            }
            throw new InvalidOperationException($"Unknown message ID: {messageId}");
        }

        public virtual int GetMessageTypeId(byte[] data)
        {
            string json;
            try
            {
                json = Encoding.UTF8.GetString(data);
            }
            catch (DecoderFallbackException)
            {
                throw new Exception("Invalid UTF-8 data received.");
            }

            //to do: 这句有问题
            var obj = deserializer.ToObject<JNetMessage>(json);
            var messageId = obj.TypeId;
            return messageId;
        }
    }
}