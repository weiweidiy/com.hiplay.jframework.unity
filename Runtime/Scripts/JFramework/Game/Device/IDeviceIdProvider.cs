namespace JFramework.Game
{
    public interface IDeviceIdProvider
    {
        /// <summary>
        /// 获取当前设备唯一ID
        /// </summary>
        /// <returns>设备唯一ID字符串</returns>
        string GetDeviceId();
    }
}