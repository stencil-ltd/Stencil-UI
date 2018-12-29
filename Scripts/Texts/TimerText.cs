using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Texts
{
    [RequireComponent(typeof(Text))]
    public class TimerText : MonoBehaviour
    {
        [Bind] private Text _text;

        public double elapsed { get; private set; }

        private void Awake()
        {
            this.Bind();
        }

        private void Update()
        {
            elapsed += Time.deltaTime;
            var elapsedInt = (int) elapsed;
            var sub = (int) ((elapsed - elapsedInt) * 100);
            _text.text = $"{elapsedInt:D2}:{sub:D2}";
        }
    }
}