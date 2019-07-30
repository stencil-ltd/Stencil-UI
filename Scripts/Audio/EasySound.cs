using System;
using System.Linq;
using Binding;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

#if UNITY_EDITOR
using UnityEditorInternal;
#endif

namespace UI
{
    [RequireComponent(typeof(AudioSource))]
    [ExecuteInEditMode]
    public class EasySound : MonoBehaviour
    {
        public static AudioMixer Mixer;
        
        public static AudioMixerGroup Master;
        public static AudioMixerGroup Music;
        public static AudioMixerGroup Sfx;

        [Header("UI")] 
        public SmartSfx sfx;
        public AudioClip Clip;
        
        [Header("Legacy")]
        [Obsolete] public bool PlayOnAwake;
        [Obsolete] public bool Loop;
        [Obsolete] public SoundType Type = SoundType.Sfx;
        
        [Bind] 
        public AudioSource Source { get; private set; }

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

        private void Awake()
        {
            this.Bind();
            
            #if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UpdateSource();
                InternalEditorUtility.SetIsInspectorExpanded(Source, false);
            }
            #endif
        }

        private void OnEnable()
        {
            if (Application.isPlaying && (sfx?.playOnAwake ?? PlayOnAwake))
                Source.Play();
        }

        public void Play()
        {
            if (Source != null) Source.Play();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying)
                UpdateSource();
        }   

        private void UpdateSource()
        {
            Source.playOnAwake = false;
            Source.loop = sfx?.loop ?? Loop;
            Source.clip = sfx?.clip ?? Clip;
            switch (sfx?.type ?? Type)
            {
                case SoundType.Sfx:
                    Source.outputAudioMixerGroup = Sfx;
                    break;
                case SoundType.Music:
                    Source.outputAudioMixerGroup = Music;
                    break;
                case SoundType.None:
                    Source.outputAudioMixerGroup = null;
                    break;
            }
        }
#endif 
    }

    [Serializable]
    public enum SoundType
    {
        Sfx, Music, None
    }
}