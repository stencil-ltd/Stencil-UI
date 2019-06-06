using System;
using System.Collections;
using Scripts.Maths;
using Scripts.Prefs;
using UnityEngine;
using Widgets;

namespace UI.Bonus
{
    public class BonusMeter : MonoBehaviour
    {
        [Header("Config")]
        public string key = "__bonus_meter_amount";
        public int pointsPerLevel = 100;

        [Header("UI")] 
        public FillBar bar;
        
        public Func<IEnumerator> onReward;

        private bool _isProcessing;
        private bool _isRewarding;
        private bool _needsReward;

        private int Queue
        {
            get => StencilPrefs.Default.GetInt($"{key}_queue");
            set => StencilPrefs.Default.SetInt($"{key}_queue", value).Save();
        }
        
        private int MarkedProgress
        {
            get => StencilPrefs.Default.GetInt($"{key}_mark");
            set => StencilPrefs.Default.SetInt($"{key}_mark", value).Save();
        }
        
        private void Awake()
        {
            bar.SetAmount(MarkedProgress, pointsPerLevel);
        }

        private void OnEnable()
        {
            StartCoroutine(ProcessQueue());
        }

        public IEnumerator Add(int points)
        {
            Queue += points;
            yield return ProcessQueue();
        }

        public IEnumerator Await()
        {
            yield return new WaitUntil(() => !_isProcessing); 
        }

        private IEnumerator ProcessQueue()
        {
            var chunk = Queue.AtMost(pointsPerLevel - MarkedProgress);
            if (chunk > 0)
            {
                bar.amount += chunk;
                MarkedProgress += chunk;
                Queue -= chunk;
            }

            if (_isProcessing)
            {
                yield return Await();
                yield break;
            }
            
            _isProcessing = true;
            yield return bar.Await();

            if (!_isRewarding && MarkedProgress == pointsPerLevel)
            {
                yield return Reward();
                _isProcessing = false;
                yield return ProcessQueue();
            }
            _isProcessing = false;
        }

        private IEnumerator Reward()
        {
            _isRewarding = true;
            yield return onReward?.Invoke();
            bar.SetAmount(0);
            MarkedProgress = 0;
            _isRewarding = false;
        }
    }
}