using Binding;
using UnityEngine;

namespace Shaders
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ShaderSwap : MonoBehaviour
    {
        public Shader swap;
        private Shader _orig;

        [Bind] 
        private MeshRenderer _render;

        public bool Swapped
        {
            get { return _render.material.shader != _orig; }
            set { _render.material.shader = value ? swap : _orig; }
        }

        private void Awake()
        {
            this.Bind();
            _orig = _render.material.shader;
        }
    }
}