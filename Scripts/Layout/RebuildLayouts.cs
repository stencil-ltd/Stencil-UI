using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Stencil.UI
{
    [ExecuteAlways]
    public class RebuildLayouts : MonoBehaviour
    {
        public RectTransform[] rebuildOnStart;
        
        private IEnumerator Start()
        {
            yield return Rebuild();
        }
        
        #if UNITY_EDITOR
        private void Update()
        {
            StartCoroutine(Rebuild());
        }
        #endif

        private IEnumerator Rebuild()
        {
            yield return null;
            foreach (var rectTransform in rebuildOnStart)
            {
                if (!rectTransform) continue;
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            }
        }
    }
}