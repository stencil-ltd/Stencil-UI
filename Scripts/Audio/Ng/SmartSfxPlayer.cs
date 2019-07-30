using UI;
using UnityEngine;

namespace Scripts.Audio.Ng
{
    public class SmartSfxPlayer : MonoBehaviour
    {
        public SmartSfx sfx;

        private void OnEnable()
        {
            if (sfx.playOnAwake) sfx.Play();
        }
    }
}