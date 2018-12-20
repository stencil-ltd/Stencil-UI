using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Texts
{
    [RequireComponent(typeof(Text))]
    public class AnimateEllipses : MonoBehaviour
    {
        public int ellipses = 3;
        public float interval = 0.5f;
        
        [Bind] private Text _text;

        private string _startText;
        private float _remaining;
        private int _count;

        private void Awake()
        {
            this.Bind();
            _startText = _text.text;
        }

        private void OnEnable()
        {
            _remaining = interval;
        }

        private void Update()
        {
            _remaining -= Time.deltaTime;
            if (_remaining < 0)
            {
                Refresh();
                _remaining = interval;
            }
        }

        private void Refresh()
        {
            var total = ellipses + 1;
            _count = (_count + 1) % total;
            var suffix = "";
            var i = 0;
            while (i++ < _count) 
                suffix += ".";
            _text.text = _startText + suffix;
        }
    }
}