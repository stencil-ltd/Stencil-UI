using JetBrains.Annotations;
using UI;
using UnityEditor.U2D;
using UnityEngine;

namespace Standard.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SfxOneShot : Controller<SfxOneShot>
    {
        public static bool Enabled = true;
        
        private AudioSource[] _sources;
        private int _index;

        private void Awake()
        {
            _sources = GetComponents<AudioSource>();
        }

        public void Play([CanBeNull] AudioClip clip)
        {
            if (!Enabled) return;
            if (clip == null) return;
            AcquireSource().PlayOneShot(clip);
        }

        private AudioSource AcquireSource()
        {
            var source = _sources[_index];
            _index = (_index + 1) % _sources.Length;
            return source;
        }

        public void Play(SmartSfx sfx)
        {
            var source = AcquireSource();
            source.outputAudioMixerGroup = sfx.MixerGroup();
        }
    }
}