
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework.Unity
{
    public interface ISpriteManager
    {
        UniTask PreloadSprites(List<string> spritesList);
        Sprite GetSprite(string name);
        UniTask<Sprite> LoadSpriteAsync(string location);
    }
}
