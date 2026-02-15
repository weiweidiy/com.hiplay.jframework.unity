using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JFramework.Unity
{
    public enum SceneMode
    {
        Single,
        Additive
    }


    /// <summary>
    /// 资源加载器接口,提供加载场景和预制体的功能,具体实现可以使用Addressable或者Resources等方式
    /// </summary>
    public interface IAssetsLoader
    {
        Task<Scene> LoadSceneAsync(string sceneName, SceneMode mode);

        Task<GameObject> InstantiateAsync(string location);

        Task<GameObject> InstantiateAsync(string location, Transform parent);

        Task<T> LoadAssetAsync<T>(string location) where T : Object ;
    }
}

