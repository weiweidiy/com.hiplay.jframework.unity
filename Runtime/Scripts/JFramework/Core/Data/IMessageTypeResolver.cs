using System;

namespace JFramework
{
    /// <summary>
    /// 消息类型解析器接口
    /// </summary>
    public interface IMessageTypeResolver
    {
        /// <summary>
        /// 根据消息数据解析消息类型
        /// </summary>
        /// <param name="data">消息数据</param>
        /// <returns>消息类型</returns>
        Type ResolveMessageType(byte[] data);

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        IMessageTypeResolver RegisterMessageType(int messageId, Type messageType);
    }
}
