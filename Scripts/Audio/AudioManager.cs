using System;
using UnityEngine;

namespace Audio
{
    public class AudioManager
    {
        public static readonly AudioManager Instance = new AudioManager();
        
        public event EventHandler<bool> OnSfxChanged;
        public event EventHandler<bool> OnMusicChanged;
        
        public bool SoundEnabled
        {
            get { return PlayerPrefs.GetInt("audio_sound_disabled") != 1; }
            set
            {
                if (SoundEnabled == value) return;
                PlayerPrefs.SetInt("audio_sound_disabled", value ? 0 : 1);
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
                OnMusicChanged?.Invoke(this, value);
            }
        }
    }
}