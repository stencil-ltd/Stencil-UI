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

        private void OnDisable()
        {
            _isProcessing = false;
            _isRewarding = false;
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

        private IEnumerator ProcessQueue(bool force = false)
        {
            Debug.Log("Process Queue");
            var chunk = Queue.AtMost(pointsPerLevel - MarkedProgress);
            if (chunk > 0)
            {
                bar.amount += chunk;
                MarkedProgress += chunk;
                Queue -= chunk;
            }

            if (!force && _isProcessing)
            {
                Debug.Log("Process Queue: Awaiting existing operation...");
                yield return Await();
                Debug.Log("Process Queue: Finished existing operation");
                yield break;
            }
            
            Debug.Log("Process Queue: Await Bar...");
            _isProcessing = true;
            yield return bar.Await();
            Debug.Log("Process Queue: Bar Complete");

            if (!_isRewarding && MarkedProgress == pointsPerLevel)
            {
                Debug.Log("Process Queue: Reward...");
                yield return Reward();
                Debug.Log("Process Queue: Reward Complete");
                yield return ProcessQueue(true);
            }
            else
            {
                _isProcessing = false;
                Debug.Log("Process Queue: Exit");
            }
        }

        private IEnumerator Reward()
        {
            Debug.Log("Bonus Reward");
            _isRewarding = true;
            yield return onReward?.Invoke();
            Debug.Log("Bonus Reward Invoked");
            bar.SetAmount(0);
            MarkedProgress = 0;
            _isRewarding = false;
        }
    }
}