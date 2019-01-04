using UnityEngine;

namespace Unity
{
    [DisallowMultipleComponent]
    public class TimeScale : MonoBehaviour
    {
        public float scale = 1f;

        private float _scale;

        private void OnEnable()
        {
            _scale = Time.timeScale;
            Time.timeScale = scale;
            Debug.Log($"Setting timeScale to {Time.timeScale}");
        }

        private void OnDisable()
        {
            Time.timeScale = _scale;
            Debug.Log($"Setting timeScale to {Time.timeScale}");
        }
    }
}