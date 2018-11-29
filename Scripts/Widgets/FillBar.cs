using JetBrains.Annotations;
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
        public Image track;
        public Image fill;
        public Text text;

        [Header("Config")] 
        public string textFormat = "{0}";
        public float smoothing = 5f;

        [CanBeNull] public string ForceText { get; private set; }
        public float CurrentAmount { get; private set; }
        public float CurrentNorm => Mathf.Clamp(CurrentAmount / max, 0, 1);
        
        private void Awake()
        {
            CurrentAmount = amount;
        }
        
        private void Update()
        {
            if (!enabled) return;
            var smooth = Application.isPlaying ? Time.deltaTime * smoothing : 1f;
            CurrentAmount = Mathf.Lerp(CurrentAmount, amount, smooth);
            var norm = CurrentNorm;
            fill.fillAmount = norm;
            text.text = string.Format(textFormat, amount);
        }

        public void SetAmounts(int amount, int max)
        {
            this.amount = CurrentAmount = amount;
            this.max = max;
        }
    }
}