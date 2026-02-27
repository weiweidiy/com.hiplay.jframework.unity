using JFramework.Game;
using System.Threading.Tasks;
using UnityEngine;

namespace JFramework.Unity
{
    public class DefaultConfigLoader : IConfigLoader
    {
        public async Task<byte[]> LoadBytesAsync(string location)
        {
            // 尝试从 Resources 加载 TextAsset
            var request = Resources.LoadAsync<TextAsset>(location);
            while (!request.isDone)
            {
                await Task.Yield();
            }
            var asset = request.asset as TextAsset;
            if (asset == null)
            {
                Debug.LogError($"Config file not found at: {location}");
                return null;
            }
            return asset.bytes;
        }
    }
}