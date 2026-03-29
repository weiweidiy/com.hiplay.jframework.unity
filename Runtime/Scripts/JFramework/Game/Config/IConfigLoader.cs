using System.Threading.Tasks;

namespace JFramework.Game
{
    public interface IConfigLoader
    {
        Task<byte[]> LoadBytesAsync(string location);
    }


}