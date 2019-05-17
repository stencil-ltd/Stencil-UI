using System.Collections;
using UnityEngine;

namespace Lifecycle
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeIn : MonoBehaviour
    {
        public float seconds = 0.3f;
        public float staggerChildren = 0.1f;

        private void OnEnable()
        {
            StartCoroutine(_Enable());
        }

        private IEnumerator _Enable()
        {
            var cg = GetComponent<CanvasGroup>();
            cg.alpha = 0f;
            LeanTween.alphaCanvas(cg, 1f, seconds);
            
            if (staggerChildren > 0f)
            {
                var cgs = GetComponentsInChildren<CanvasGroup>();
                foreach (var group in cgs)
                {
                    if (cg == group) continue;
                    group.alpha = 0f;
                }

                yield return new WaitForSeconds(staggerChildren);
                foreach (var group in cgs)
                {
                    if (cg == group) continue;
                    LeanTween.alphaCanvas(group, 1f, seconds);
                    yield return new WaitForSeconds(staggerChildren);
                }
            }
        }
    }
}