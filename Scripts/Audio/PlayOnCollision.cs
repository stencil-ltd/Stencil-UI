using Binding;
using Standard.Audio;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(Collider))]
    [ExecuteInEditMode]
    public class PlayOnCollision : MonoBehaviour
    {
        public bool Repeatable;
        public bool IsTrigger;
        public string ColliderTag;

        public bool Used { get; set; }

        [Header("Sound Choices")]
        public AudioSource Sound;
        public AudioClip Clip;
        
        [Bind] public Collider Collider { get; private set; }

        private void Awake()
        {
            this.Bind();
            Sound = Sound ?? GetComponent<AudioSource>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (IsTrigger) return;
            if (!string.IsNullOrEmpty(ColliderTag) && !other.gameObject.CompareTag(ColliderTag)) return;
            Play();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsTrigger) return;
            if (!string.IsNullOrEmpty(ColliderTag) && !other.gameObject.CompareTag(ColliderTag)) return;
            Play();
        }

        private void Play()
        {
            if (Used && !Repeatable) return;
            Used = true;
            if (Sound != null)
            {
                Debug.Log($"Playing {Sound.clip}");
                Sound.Play();
            }

            if (Clip != null)
            {
                Debug.Log($"Playing {Clip}");
                SfxOneShot.Instance.Play(Clip);
            }
        }
    }
}