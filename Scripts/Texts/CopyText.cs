using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Texts
{
    [RequireComponent(typeof(Text))]
    public class CopyText : MonoBehaviour
    {
        public Text text;

        [Bind]
        private Text _text;
        
        private void Awake()
        {
            this.Bind();
        }

        private void Update()
        {
            _text.text = text.text;
        }
    }
}