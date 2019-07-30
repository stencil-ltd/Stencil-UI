using System;
using Binding;
using UnityEngine;

namespace Audio
{
    public enum AudioType
    {
        Music, Sound
    }
    
    [RequireComponent(typeof(AudioSource))]
    public class AudioHelper : MonoBehaviour
    {
        [Bind]
        private AudioSource _audio;

        public AudioType AudioType;
        
        private void Awake()
        {
            this.Bind();
            AudioManager.Instance.OnMusicChanged += OnChanged;
            AudioManager.Instance.OnSfxChanged += OnChanged;
            OnChanged();
        }

        private void OnDestroy()
        {
            AudioManager.Instance.OnMusicChanged -= OnChanged;
            AudioManager.Instance.OnSfxChanged -= OnChanged;
        }

        private void OnChanged(object sender, bool e)
        {
            OnChanged();
        }

        private void OnChanged()
        {
            switch (AudioType)
            {
                case AudioType.Music:
                    _audio.volume = AudioManager.Instance.MusicEnabled ? 1f : 0f;
                    break;
                case AudioType.Sound:
                    _audio.volume = AudioManager.Instance.SoundEnabled ? 1f : 0f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}