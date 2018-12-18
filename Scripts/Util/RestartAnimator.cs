using System.Collections;
using Binding;
using UnityEngine;

namespace Util
{
    public class RestartAnimator : MonoBehaviour
    {
        public string trigger = "Normal";
        
        [Bind]
        public Animator Animator { get; private set; }
        
        private void OnEnable()
        {
            this.Bind();
            Animator.SetTrigger(trigger);
//            Animator.Rebind();
//            StartCoroutine(_DoIt());
        }

        private IEnumerator _DoIt()
        {
            if (!Animator.enabled) yield break;
            Animator.enabled = false;
            yield return null;
            Animator.enabled = true;
        }
    }
}