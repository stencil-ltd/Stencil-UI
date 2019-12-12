using UnityEngine;

namespace Stencil.Ui.Aggregate
{
    public abstract class AggregateWidget : MonoBehaviour
    {
        public GameObject[] aggregate;
        public bool invert;
        public bool ignoreHiddenParents;

        public bool ShouldEnable()
        {
            var active = true;
            foreach (var o in aggregate)
            {
                var ok = o.activeSelf || (ignoreHiddenParents && !o.transform.parent.gameObject.activeInHierarchy);
                active &= ok;
            }
            return active ^ invert;
        }
        
    }
}