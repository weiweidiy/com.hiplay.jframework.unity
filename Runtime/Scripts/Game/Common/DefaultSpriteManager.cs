
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework.Unity
{
    public class DefaultSpriteManager : ISpriteManager
    {
        IAssetsLoader assetsLoader;

        Dictionary<string, Sprite> spritesCache = new Dictionary<string, Sprite>();

        public DefaultSpriteManager(IAssetsLoader assetsLoader)
        {
            this.assetsLoader = assetsLoader;
        }

        public async UniTask Initialize(List<string> spritesList)
        {
            var tasks = new List<UniTask>();
            foreach (var texture in spritesList)
            {
                //Debug.Log("---- Load Texture  --------- " + texture);
                var location = texture;
                if (spritesCache.ContainsKey(location))
                    continue;
                var task = LoadSpriteAsync(location, (key, sprite) => {

                    if (sprite != null && !spritesCache.ContainsKey(key))
                    {
                        spritesCache.Add(key, sprite);
                    }
                    else
                    {
                        Debug.LogError($"Failed to load texture at {key}");
                    }
                });
                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
        }

        private async UniTask LoadSpriteAsync(string location, Action<string, Sprite> onComplete)
        {
            var sprite = await LoadSpriteAsync(location);
            onComplete?.Invoke(location, sprite);
        }


        public Sprite GetSprite(string name)
        {
            if (spritesCache.TryGetValue(name, out var sprite))
            {
                return sprite;
            }
            else
            {
                Debug.LogError($"Sprite not found for key: {name}");
                return null;
            }
        }



        public async UniTask<Sprite> LoadSpriteAsync(string location)
        {
            var sprite = await assetsLoader.LoadAssetAsync<Sprite>(location);
            return sprite;
        }
    }
}
