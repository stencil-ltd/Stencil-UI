using UnityEngine;

namespace Physic
{
    public class ConstantVelocity : MonoBehaviour
    {
        public Vector3 Velocity;

        public float x
        {
            get => Velocity.x;
            set => Velocity.x = value;
        }

        public float y
        {
            get => Velocity.y;
            set => Velocity.y = value;
        }

        public float z
        {
            get => Velocity.z;
            set => Velocity.z = value;
        }

        private void Update()
        {
            var pos = transform.position;
            pos += Velocity * Time.deltaTime;
            transform.position = pos;
        }
    }
}