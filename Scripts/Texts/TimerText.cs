using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Texts
{
    [RequireComponent(typeof(Text))]
    public class TimerText : MonoBehaviour
    {
        [Bind] private Text _text;

        private double _elapsed;

        private void Awake()
        {
            this.Bind();
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;
            var elapsedInt = (int) _elapsed;
            var sub = (_elapsed - elapsedInt) * 100;
            _text.text = $"{elapsedInt}:{sub:N0}";
        }
    }
}