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
        public MeshRenderer Renderer { get; private set; }

        public bool Swapped
        {
            get { return Renderer.material.shader != _orig; }
            set { Renderer.material.shader = value ? swap : _orig; }
        }

        private void Awake()
        {
            this.Bind();
            _orig = Renderer.material.shader;
        }
    }
}