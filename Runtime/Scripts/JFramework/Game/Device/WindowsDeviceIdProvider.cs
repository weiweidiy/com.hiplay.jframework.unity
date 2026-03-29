#if Windows
using System.Management;

namespace JFramework.Game
{
    public class WindowsDeviceIdProvider : IDeviceIdProvider
    {
        public string GetDeviceId()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        var uuid = obj["UUID"]?.ToString();
                        if (!string.IsNullOrEmpty(uuid))
                        {
                            return uuid;
                        }
                    }
                }
            }
            catch
            {
                // 可以根据需要记录日志或抛出异常
            }
            return string.Empty;
        }
    }
}
#endif