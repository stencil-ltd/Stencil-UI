using System;
using System.Collections;
using JetBrains.Annotations;
using Scripts.Maths;
using StencilEvents;
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
        public Image[] fills;
        [CanBeNull] public Text text;
        public FloatEvent onFinished;

        [Header("Config")] 
        public bool showPercent = false;
        public string textFormat = "{0}/{1}";
        public float smoothing = 5f;
        public int segments = 0;
        public AnimationCurve normCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public bool IsAnimating => !IsPaused && !IsFinished;
        public bool IsPaused { get; private set; }
       
        public bool IsFinished => CurrentAmount.IsAbout(amount); 

        private float _lastTier = 0;

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

        private float _norm;

        private void Awake()
        {
            CurrentAmount = amount;
        }
        
        private void Update()
        {
//            if (!enabled || !IsAnimating) return;
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
            SetNorm(norm);
        }

        private void SetNorm(float norm)
        {
            var changed = _norm != norm;
            _norm = norm;
            foreach (var fill in fills) fill.fillAmount = norm;
            if (changed && IsFinished)
            {
//                Debug.Log($"FillBar Finished ({(int)(norm * 100)}%)");
                CurrentAmount = amount;
                onFinished?.Invoke(amount);
            }
        }

        private void UpdateText()
        {
            if ((object) text != null)
            {
                if (!string.IsNullOrEmpty(forceText))
                    text.text = forceText;
                else
                {
                    text.text = showPercent ? 
                        string.Format(textFormat, Mathf.RoundToInt(CurrentNorm * 100)) : 
                        string.Format(textFormat, amount, max);
                }
            }
        }

        public IEnumerator Await()
        {
            while (IsAnimating)
                yield return null;
        }

        public void SetAmount(float amount) => SetAmount(amount, max);
        public void SetAmount(float amount, float max) => SetAmount(amount, max, min);
        public void SetAmount(float amount, float max, float min)
        {
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