using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using JFramework.Unity;
using DG.Tweening;

namespace Game.Common
{
    public class DefaultAudioManager : IGameAudioManager
    {
        private AudioSource musicSource;
        private List<AudioSource> soundSources = new List<AudioSource>();
        private float soundVolume = 1f;
        private float musicVolume = 1f;
        private bool isMuted = false;
        private Tweener musicFadeTween; // 淡入淡出动画句柄

        IAssetsLoader assetsLoader;

        public DefaultAudioManager(IAssetsLoader assetsLoader)
        {
            this.assetsLoader = assetsLoader;
        }

        public void Initialize()
        {
            var go = new GameObject("GameAudioManager_MusicSource");
            Object.DontDestroyOnLoad(go);
            musicSource = go.AddComponent<AudioSource>();
            musicSource.loop = true;
        }

        public async Task PlaySound(string clipName)
        {
            var clip = await LoadAudioClip(clipName);
            if (clip == null) return;

            var go = new GameObject($"Sound_{clipName}");
            var source = go.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = isMuted ? 0f : soundVolume;
            source.Play();
            soundSources.Add(source);

            Object.Destroy(go, clip.length);
        }

        /// <summary>
        /// 播放背景音乐，支持淡入淡出
        /// </summary>
        /// <param name="clipName">音乐名称或资源路径</param>
        /// <param name="loop">是否循环</param>
        /// <param name="fadeDuration">淡入淡出时长（秒），默认0.5秒</param>
        public async Task PlayMusic(string clipName, bool loop = true, float fadeDuration = 0.5f)
        {
            var clip = await LoadAudioClip(clipName);
            if (clip == null) return;

            // 如果当前有音乐在播放，先淡出
            if (musicSource.isPlaying && musicSource.clip != null)
            {
                musicFadeTween?.Kill();
                
                musicFadeTween = musicSource.DOFade(0f, fadeDuration).OnComplete(() =>
                {
                    musicSource.Stop();
                    musicSource.clip = clip;
                    musicSource.loop = loop;
                    musicSource.volume = 0f;
                    musicSource.Play();
                    // 淡入新音乐
                    musicFadeTween = musicSource.DOFade(isMuted ? 0f : musicVolume, fadeDuration);
                });
            }
            else
            {
                musicSource.clip = clip;
                musicSource.loop = loop;
                musicSource.volume = 0f;
                musicSource.Play();
                musicFadeTween?.Kill();
                musicFadeTween = musicSource.DOFade(isMuted ? 0f : musicVolume, fadeDuration);
            }
        }

        public void StopMusic()
        {
            musicFadeTween?.Kill();
            musicSource.Stop();
            musicSource.clip = null;
        }

        public void PauseAll()
        {
            musicSource.Pause();
            foreach (var source in soundSources)
            {
                if (source != null) source.Pause();
            }
        }

        public void ResumeAll()
        {
            musicSource.UnPause();
            foreach (var source in soundSources)
            {
                if (source != null) source.UnPause();
            }
        }

        public void SetSoundVolume(float volume)
        {
            soundVolume = Mathf.Clamp01(volume);
            foreach (var source in soundSources)
            {
                if (source != null) source.volume = isMuted ? 0f : soundVolume;
            }
        }

        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            if (musicSource != null)
            {
                if (musicFadeTween != null && musicFadeTween.IsActive())
                {
                    // 如果正在淡入淡出，直接设置目标值
                    musicFadeTween.ChangeEndValue(isMuted ? 0f : musicVolume, true);
                }
                else
                {
                    musicSource.volume = isMuted ? 0f : musicVolume;
                }
            }
        }

        public void SetMute(bool mute)
        {
            isMuted = mute;
            if (musicSource != null)
            {
                musicSource.volume = mute ? 0f : musicVolume;
            }
            foreach (var source in soundSources)
            {
                if (source != null) source.volume = mute ? 0f : soundVolume;
            }
        }

        private async Task<AudioClip> LoadAudioClip(string clipName)
        {
            var clip = await assetsLoader.LoadAssetAsync<AudioClip>(clipName);
            //var clip = Resources.Load<AudioClip>(clipName);
            if (clip == null)
            {
                Debug.LogWarning($"音频资源未找到: {clipName}");
            }
            return clip;
        }
    }
}