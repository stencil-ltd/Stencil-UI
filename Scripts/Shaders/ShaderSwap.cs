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

        public Shader CurrentShader => Renderer.material.shader;
        public bool Swapped
        {
            get { return CurrentShader != _orig; }
            set { Renderer.material.shader = value ? swap : _orig; }
        }

        private void Awake()
        {
            this.Bind();
            _orig = Renderer.material.shader;
        }
    }
}