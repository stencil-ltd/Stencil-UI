using System;
using UnityEngine;
using UnityEngine.Audio;
using Util;

namespace Scripts.Audio
{
    [CreateAssetMenu(menuName = "Stencil/Audio System")]
    public class AudioSystem2 : Singleton<AudioSystem2>
    {
        public AudioMixer mixer;
        
        public event EventHandler<bool> OnSfxChanged;
        public event EventHandler<bool> OnMusicChanged;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void OnLoad()
        {
            Instance?.UpdateMixers();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (Application.isPlaying)
                UpdateMixers();
        }

        public bool SoundEnabled
        {
            get { return PlayerPrefs.GetInt("audio_sound_disabled") != 1; }
            set
            {
                if (SoundEnabled == value) return;
                PlayerPrefs.SetInt("audio_sound_disabled", value ? 0 : 1);
                PlayerPrefs.Save();
                UpdateMixers();
                OnSfxChanged?.Invoke(this, value);
            }
        }
        
        public bool MusicEnabled
        {
            get { return PlayerPrefs.GetInt("audio_music_disabled") != 1; }
            set
            {
                if (MusicEnabled == value) return;
                PlayerPrefs.SetInt("audio_music_disabled", value ? 0 : 1);
                PlayerPrefs.Save();
                UpdateMixers();
                OnMusicChanged?.Invoke(this, value);
            }
        }

        public void UpdateMixers()
        {
            mixer.SetFloat("Sfx", SoundEnabled ? 0f : -80f);
            mixer.SetFloat("Music", MusicEnabled ? 0f : -80f);
        }
    }
}