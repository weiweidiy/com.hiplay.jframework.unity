using System.IO;
using System.Threading.Tasks;

namespace JFramework.Game
{
    public class LocalFileConfigLoader : IConfigLoader
    {
        public async Task<byte[]> LoadBytesAsync(string location)
        {
            if (string.IsNullOrEmpty(location) || !File.Exists(location))
            {
                throw new FileNotFoundException($"配置文件未找到: {location}");
            }

            return await Task.Run(() => File.ReadAllBytes(location)).ConfigureAwait(false);
        }
    }
}