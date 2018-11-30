using JetBrains.Annotations;
using Scripts.Maths;
using UnityEngine;
using UnityEngine.UI;

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

        [Header("Config")] 
        public string textFormat = "{0}";
        public float smoothing = 5f;
        public AnimationCurve normCurve = AnimationCurve.Linear(0, 0, 1, 1);

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
            if (!enabled) return;
            UpdateFill();
            UpdateText();
        }

        private void UpdateFill()
        {
            var smooth = Application.isPlaying ? Time.deltaTime * smoothing : 1f;
            CurrentAmount = Mathf.Lerp(CurrentAmount, amount, smooth);
            var norm = CurrentNorm;
            fill.fillAmount = norm;
        }

        private void UpdateText()
        {
            if (text != null)
            {
                if (!string.IsNullOrEmpty(forceText))
                    text.text = forceText;
                else
                    text.text = string.Format(textFormat, amount);
            }
        }

        public void SetAmount(float amount) => SetAmount(amount, max);
        public void SetAmount(float amount, float max) => SetAmount(amount, max, min);
        public void SetAmount(float amount, float max, float min)
        {
            this.amount = CurrentAmount = amount;
            this.max = max;
            this.min = min;
        }

        public void ForceTextValue([CanBeNull] string text)
        {
            forceText = text;
        }
    }
}