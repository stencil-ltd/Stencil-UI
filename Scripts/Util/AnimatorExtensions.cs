using System.Collections;
using UnityEngine;

namespace Scripts.Util
{
    public static class AnimatorExtensions
    {
        public static IEnumerator Await(this Animator anim, float leadTime = 0f)
        {
            yield return null;
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime - leadTime);
        }
    }
}