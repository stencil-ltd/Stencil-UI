using System;
using JetBrains.Annotations;
using Scripts.Maths;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Util;

namespace Widgets
{
    public class FillBar : MonoBehaviour
    {
        [Header("Data")]
        public float amount = 0.5f;
        public float min = 0f;
        public float max = 1f;

        [Header("UI")]
        public Image fill;
        [CanBeNull] public Text text;
        public Image[] extraFills = {};
        public UnityEvent onFinish;

        [Header("Config")] 
        public string textFormat = "{0}/{1}";
        public float smoothing = 5f;
        public int segments = 0;
        public AnimationCurve normCurve = AnimationCurve.Linear(0, 0, 1, 1);

        private bool _active;

        [CanBeNull] public string forceText;
        public float CurrentAmount { get; private set; }
        public float CurrentNorm
        {
            get
            {
                var retval = Mathf.Clamp((CurrentAmount - min) / (max - min), 0, 1);
                retval = normCurve.Evaluate(retval);
                return retval;
            }
        }

        private void Awake()
        {
            CurrentAmount = amount;
        }
        
        private void Update()
        {
            if (!enabled || !_active) return;
            UpdateFill();
            UpdateText();
        }

        private void UpdateFill()
        {
            var smooth = Application.isPlaying ? Time.deltaTime * smoothing : 1f;
            CurrentAmount = Mathf.Lerp(CurrentAmount, amount, smooth);
            var norm = CurrentNorm;
            if (segments > 0)
            {
                var small = (int) (norm * segments);
                norm = (float) small / segments;
            }
            fill.fillAmount = norm;
            foreach (var extraFill in extraFills) 
                extraFill.fillAmount = norm;
            if (norm >= 1f)
            {
                _active = false;
                onFinish?.Invoke();
            }
        }

        private void UpdateText()
        {
            if ((object) text != null)
            {
                if (!string.IsNullOrEmpty(forceText))
                    text.text = forceText;
                else
                    text.text = string.Format(textFormat, amount, max);
            }
        }

        public void SetAmount(float amount) => SetAmount(amount, max);
        public void SetAmount(float amount, float max) => SetAmount(amount, max, min);
        public void SetAmount(float amount, float max, float min)
        {
            _active = true;
            this.amount = CurrentAmount = amount;
            this.max = max;
            this.min = min;
            UpdateFill();
            UpdateText();
        }

        public void ForceTextValue([CanBeNull] string text)
        {
            forceText = text;
            UpdateText();
        }
    }
}