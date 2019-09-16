using Binding;
using UnityEngine;

namespace Stencil.UI.SafeArea
{
    public class Notcher : MonoBehaviour
    {
        public bool undoNotch;
        
        public float topScale = 1f;
        public float bottomScale = 1f;
        
        [Bind] 
        private RectTransform _rect;

        private bool _updated;
        
        private void Awake()
        {
            this.Bind();
        }
        
        private void Start()
        {
            ApplyOffset();
        }

        private void ApplyOffset()
        {
            if (_updated) return;
            _updated = true;
            
            var top = NotchRoot.Instance.TopSafePadding * topScale;
            var bot = NotchRoot.Instance.BottomSafePadding * bottomScale;

            var min = _rect.anchorMin;
            var max = _rect.anchorMax;

            if (!undoNotch)
            {
                top *= -1;
                bot *= -1;
            }

            if (min.y == 0)
            {
                Debug.Log("Ignore Safe Area Min");
                var offset = _rect.offsetMin;
                offset.y -= bot;
                _rect.offsetMin = offset;
            }
            else if (min.y == 1)
            {
                var offset = _rect.offsetMin;
                offset.y += top;
                _rect.offsetMin = offset;
            }

            if (max.y == 0)
            {
                Debug.Log("Ignore Safe Area Max");
                var offset = _rect.offsetMax;
                offset.y -= bot;
                _rect.offsetMax = offset;
            }
            else if (max.y == 1)
            {
                Debug.Log("Ignore Safe Area Max");
                var offset = _rect.offsetMax;
                offset.y += top;
                _rect.offsetMax = offset;
            }
        }
    }
}