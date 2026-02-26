using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JFramework.Unity
{
    public class DefaultAssetsLoader : IAssetsLoader
    {
        public async Task<GameObject> InstantiateAsync(string location)
        {
            var request = Resources.LoadAsync<GameObject>(location);
            while (!request.isDone)
            {
                await Task.Yield();
            }
            var prefab = request.asset as GameObject;
            if (prefab == null)
            {
                Debug.LogError($"Prefab not found at: {location}");
                return null;
            }
            return Object.Instantiate(prefab);
        }

        public async Task<GameObject> InstantiateAsync(string location, Transform parent)
        {
            var request = Resources.LoadAsync<GameObject>(location);
            while (!request.isDone)
            {
                await Task.Yield();
            }
            var prefab = request.asset as GameObject;
            if (prefab == null)
            {
                Debug.LogError($"Prefab not found at: {location}");
                return null;
            }
            return Object.Instantiate(prefab, parent);
        }

        public async Task<T> LoadAssetAsync<T>(string assetPath) where T : UnityEngine.Object
        {
            var request = Resources.LoadAsync<T>(assetPath);
            while (!request.isDone)
            {
                await Task.Yield();
            }
            return request.asset as T;
        }

        public async Task<Scene> LoadSceneAsync(string sceneName, SceneMode mode)
        {
            var loadMode = mode == SceneMode.Additive ? LoadSceneMode.Additive : LoadSceneMode.Single;
            var operation = SceneManager.LoadSceneAsync(sceneName, loadMode);
            while (!operation.isDone)
            {
                await Task.Yield();
            }
            return SceneManager.GetSceneByName(sceneName);
        }
    }
}