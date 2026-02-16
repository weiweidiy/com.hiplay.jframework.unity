using System.Threading.Tasks;
using UnityEngine;

namespace Game.Common
{
    public interface IGameAudioManager
    {
        void Initialize();

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="clipName">音效名称或资源路径</param>
        Task PlaySound(string clipName);

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="clipName">音乐名称或资源路径</param>
        Task PlayMusic(string clipName, bool loop = true, float fadeDuration = 0.5f);

        /// <summary>
        /// 停止当前背景音乐
        /// </summary>
        void StopMusic();

        /// <summary>
        /// 暂停所有声音
        /// </summary>
        void PauseAll();

        /// <summary>
        /// 恢复所有声音
        /// </summary>
        void ResumeAll();

        /// <summary>
        /// 设置音效音量
        /// </summary>
        /// <param name="volume">0~1</param>
        void SetSoundVolume(float volume);

        /// <summary>
        /// 设置音乐音量
        /// </summary>
        /// <param name="volume">0~1</param>
        void SetMusicVolume(float volume);

        /// <summary>
        /// 静音/取消静音
        /// </summary>
        /// <param name="mute">是否静音</param>
        void SetMute(bool mute);
    }
}