using UnityEngine;

namespace Unity
{
    public class TimeScale : MonoBehaviour
    {
        public float scale = 1f;

        private float _scale;

        private void OnEnable()
        {
            _scale = Time.timeScale;
            Time.timeScale = scale;
        }

        private void OnDisable()
        {
            Time.timeScale = _scale;
        }
    }
}