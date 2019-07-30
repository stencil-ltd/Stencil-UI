using UnityEngine;

namespace Physic
{
    [ExecuteInEditMode]
    public class ConstantRotation : MonoBehaviour
    {
        public bool RunInEditor;
        
        public Vector3 Velocity;
        private Quaternion _rotation;

        private void OnEnable()
        {
            _rotation = transform.localRotation;
        }

        private void OnDisable()
        {
            transform.localRotation = _rotation;
        }

        private void Update()
        {
            if (Application.isPlaying || RunInEditor)
            transform.Rotate(Velocity * Time.deltaTime);
        }
    }
}