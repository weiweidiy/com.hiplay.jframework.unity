namespace JFramework
{
    /// <summary>
    /// 网络数据处理策略
    /// </summary>
    public interface INetworkMessageProcessStrate
    {
        byte[] ProcessOutMessage(IJNetMessage message);

        IJNetMessage ProcessComingMessage(byte[] data);
    }
}