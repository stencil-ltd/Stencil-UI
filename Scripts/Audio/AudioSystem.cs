using System;
using Plugins.UI;
using UI;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioSystem : PermanentV2<AudioSystem>
    {
        public AudioMixer Mixer;
        
        public event EventHandler<bool> OnSfxChanged;
        public event EventHandler<bool> OnMusicChanged;

        protected override void OnStart()
        {
            base.OnStart();
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

        private void UpdateMixers()
        {
            Mixer.SetFloat("Sfx", SoundEnabled ? 0f : -80f);
            Mixer.SetFloat("Music", MusicEnabled ? 0f : -80f);
        }
    }
}