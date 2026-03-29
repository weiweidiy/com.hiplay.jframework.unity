namespace JFramework
{

    public interface INetMessageSerializerStrate
    {
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>序列化后的字节数组</returns>
        byte[] Serialize(IJNetMessage obj);

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="typeResolver">类型解析器</param>
        /// <returns>反序列化后的对象</returns>
        IJNetMessage Deserialize(byte[] data, IMessageTypeResolver typeResolver);
    }
}
