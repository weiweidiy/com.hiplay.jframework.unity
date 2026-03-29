using System;

namespace JFramework
{
    /// <summary>
    /// 网络消息处理策略基类
    /// </summary>
    public class JNetworkMessageProcessStrate : INetworkMessageProcessStrate
    {

        private readonly JDataProcesserManager outProcesser;
        private readonly JDataProcesserManager comingProcesser;
        private readonly INetMessageSerializerStrate serializerStrate;
        private readonly IMessageTypeResolver typeResolver;

        /// <summary>
        /// 处理出去的消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public byte[] ProcessOutMessage(IJNetMessage message)
        {
            //转json->byte[]
            var byteMsg = GetSerializer().Serialize(message);

            //数据处理（比如加密，编码等）
            return GetDataOutProcesser() != null ? GetDataOutProcesser().GetResult(byteMsg) : byteMsg;
        }

        /// <summary>
        /// 处理收到的消息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IJNetMessage ProcessComingMessage(byte[] data)
        {
            //数据加工           
            data = GetDataComingProcesser() != null ? GetDataComingProcesser().GetResult(data) : data;

            return GetSerializer().Deserialize(data, typeResolver);

        }

        /// <summary>
        /// 获取序列化工具，子类实现
        /// </summary>
        /// <returns></returns>
        public INetMessageSerializerStrate GetSerializer() => serializerStrate;

        /// <summary>
        /// 数据出去前的处理工具
        /// </summary>
        /// <returns></returns>
        public virtual JDataProcesserManager GetDataOutProcesser() => outProcesser;

        /// <summary>
        /// 数据进来时候的处理工具
        /// </summary>
        /// <returns></returns>
        public virtual JDataProcesserManager GetDataComingProcesser() => comingProcesser;


        public JNetworkMessageProcessStrate(INetMessageSerializerStrate serializer, IMessageTypeResolver typeResolver, JDataProcesserManager outProcesser, JDataProcesserManager comingProcesser)
        {
            this.serializerStrate = serializer;
            this.outProcesser = outProcesser;
            this.comingProcesser = comingProcesser;
            this.typeResolver = typeResolver;
        }
    }
}