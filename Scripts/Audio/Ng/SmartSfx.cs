using System;
using System.Linq;
using JetBrains.Annotations;
using State;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

#if UNITY_EDITOR
using UnityEditorInternal;
#endif

namespace UI
{
    [CreateAssetMenu(menuName = "SFX/Smart Sfx")]
    public class SmartSfx : StencilData
    {
        public static AudioMixer Mixer;
        
        public static AudioMixerGroup Master;
        public static AudioMixerGroup Music;
        public static AudioMixerGroup Sfx;

        private static GameObject _parent;

        [Header("Sfx")]
        public bool loop;
        public bool playOnAwake;
        public SoundType type = SoundType.Sfx;
        public AudioClip clip;

        [CanBeNull] 
        [NonSerialized] 
        private AudioSource _source;

        #if UNITY_EDITOR
        [InitializeOnLoadMethod]
        #else
        [RuntimeInitializeOnLoadMethod]
        #endif
        public static void OnLoad()
        {
            var mixers = Resources.FindObjectsOfTypeAll<AudioMixer>();
            Mixer = mixers.FirstOrDefault(mixer => mixer.name == "Mixer");
            Sfx = Mixer?.FindMatchingGroups("Sfx")[0];
            Music = Mixer?.FindMatchingGroups("Music")[0];
            Master = Mixer?.FindMatchingGroups("Master")[0];
        }

        public void Play()
        {
            if (_source == null)
            {
                if (_parent == null)
                {
                    _parent = new GameObject("SmartSfx");
                    DontDestroyOnLoad(_parent.gameObject);
                }
                
                var obj = new GameObject(clip.name);
                DontDestroyOnLoad(obj.gameObject);
                obj.transform.SetParent(_parent.transform);
                _source = obj.AddComponent<AudioSource>();
                _source.outputAudioMixerGroup = MixerGroup();
                _source.clip = clip;
                _source.loop = loop;
                _source.playOnAwake = playOnAwake;
            }
            _source.Play();
        }

        [CanBeNull]
        public AudioMixerGroup MixerGroup()
        {
            switch (type)
            {
                case SoundType.Sfx:
                    return Sfx;
                case SoundType.Music:
                    return Music;
                case SoundType.None:
                    return null;
            }

            return null;
        }
    }
}