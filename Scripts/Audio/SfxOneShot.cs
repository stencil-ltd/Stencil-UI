using JetBrains.Annotations;
using UI;
using UnityEngine;

namespace Standard.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SfxOneShot : Controller<SfxOneShot>
    {
        private AudioSource[] _sources;
        private int _index;

        private void Awake()
        {
            _sources = GetComponents<AudioSource>();
        }

        public void Play([CanBeNull] AudioClip clip)
        {
            if (clip == null) return;
            var source = _sources[_index];
            source.PlayOneShot(clip);
            _index = (_index + 1) % _sources.Length;
        }
    }
}