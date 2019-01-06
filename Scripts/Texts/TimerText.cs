using System;
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
            var current = TimeSpan.FromSeconds(elapsed);
            var str = $"{current.Seconds:D2}:{current.Milliseconds / 10:D2}";
            var minutes = current.Minutes;
            if (minutes > 0)
                str = $"{current.Minutes:D2}:{str}";
            _text.text = str;
        }
    }
}