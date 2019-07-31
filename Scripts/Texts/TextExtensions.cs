using System;
using System.Collections;
using Dirichlet.Numerics;
using Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Util
{
    public static class TextExtensions
    {
        private static readonly AnimationCurve Curve 
            = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        
        public static void SetAmount(this Text text, string format, string numberType, UInt128 to)
        {
            text.text = string.Format(format, to.ToString(numberType));
        }
        
        public static void SetAmount(this Text text, string format, string numberType, long to)
        {
            text.text = string.Format(format, to.ToString(numberType));
        }
        
        public static void SetAmount(this Text text, string format, NumberFormats.Format numberType, UInt128 to)
        {
            text.text = string.Format(format, numberType.FormatAmount(to));
        }
        
        public static void SetAmount(this Text text, string format, NumberFormats.Format numberType, long to)
        {
            text.text = string.Format(format, numberType.FormatAmount(to));
        }
        
        public static IEnumerator LerpAmount(this Text text, string format, NumberFormats.Format numberType, UInt128 to, float duration)
        {
            NumberFormats.TryParse(text.text.ToLower().Replace(",", "").Replace("$", "").Replace("x", ""), numberType, out UInt128 from);
            var start = DateTime.UtcNow;
            for (;;)
            {
                var elapsed = (float) (DateTime.UtcNow - start).TotalSeconds;
                var percent = Curve.Evaluate(elapsed / duration);
                var normed = UInt128.Lerp(from, to, percent);
                text.SetAmount(format, numberType, normed);
                if (elapsed >= duration) yield break;
                yield return null;
            }
        }
        
        public static IEnumerator LerpAmount(this Text text, string format, string numberType, UInt128 to, float duration)
        {
            UInt128.TryParse(text.text.ToLower().Replace(",", "").Replace("$", "").Replace("x", ""), out var from);
            var start = DateTime.UtcNow;
            for (;;)
            {
                var elapsed = (float) (DateTime.UtcNow - start).TotalSeconds;
                var percent = Curve.Evaluate(elapsed / duration);
                var normed = UInt128.Lerp(from, to, percent);
                text.SetAmount(format, numberType, normed);
                if (elapsed >= duration) yield break;
                yield return null;
            }
        }
    }
}