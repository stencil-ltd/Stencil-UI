using System;
using System.Collections;
using Binding;
using Dirichlet.Numerics;
using Scripts.Util;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Texts
{
    [RequireComponent(typeof(Text))]
    public class BigNumberText : MonoBehaviour
    {
        [Header("UI")] 
        public NumberFormats.Format format = NumberFormats.Format.MassiveAmount;
        public AnimationCurve animCurve = Curves.EaseInOut;
        public string prefix = "";

        public UInt128 Amount { get; private set; }

        [Bind]
        private Text _text;

        private Coroutine _co;

        private void Awake()
        {
            this.Bind();
        }

        private void OnEnable()
        {
            Refresh();
        }

        private void LateUpdate()
        {
            Refresh();
        }

        private void Refresh()
        {
            _text.text = prefix + format.FormatAmount(Amount);
        }

        public void Set(UInt128 amount)
        {
            if (_co != null)
            {
                StopCoroutine(_co);
                _co = null;
            }
            Amount = amount;
        }

        public void Lerp(UInt128 amount, float time = 1f)
        {
            if (amount == Amount) return;
            if (_co != null) StopCoroutine(_co);
            _co = StartCoroutine(_Lerp(amount, time));
        }

        private IEnumerator _Lerp(UInt128 amount, float time)
        {
            var from = Amount;
            var start = DateTime.UtcNow;
            for (;;)
            {
                var elapsed = (float) (DateTime.UtcNow - start).TotalSeconds;
                var norm = animCurve.Evaluate(elapsed / time);
                Amount = UInt128.Lerp(from, amount, norm);
                yield return null;
            }
        }
    }
}