using UnityEngine;

namespace Audio
{
    public static class AudioExtensions
    {
        public static void PlayOneShot(this AudioSource source) => source.PlayOneShot(source.clip);
    }
}