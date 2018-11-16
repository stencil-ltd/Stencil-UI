using System.Collections;
using UnityEngine;

namespace Scripts.Util
{
    public static class AnimatorExtensions
    {
        public static IEnumerator Await(this Animator anim, float leadTime = 0f)
        {
            yield return null;
            var duration = anim.GetCurrentAnimatorStateInfo(0).length;
            var norm = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            Debug.Log($"Animator awaiting {duration} - {leadTime} [norm: {norm}]");
            yield return new WaitForSeconds(duration - leadTime);
        }
    }
}