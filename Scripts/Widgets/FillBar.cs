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
        public float max = 1f;

        [Header("UI")]
        public Image fill;
        [CanBeNull] public Text text;

        [Header("Config")] 
        public string textFormat = "{0}";
        public float smoothing = 5f;
        public float minNorm = 0f; 
        public bool logScale;

        [CanBeNull] public string forceText;
        public float CurrentAmount { get; private set; }
        public float CurrentNorm
        {
            get
            {
                var retval = Mathf.Clamp(CurrentAmount / max, minNorm, 1);
                if (logScale) retval = MathHelpers.LogNorm(retval);
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
        public void SetAmount(float amount, float max)
        {
            this.amount = CurrentAmount = amount;
            this.max = max;
        }

        public void ForceTextValue([CanBeNull] string text)
        {
            forceText = text;
        }
    }
}