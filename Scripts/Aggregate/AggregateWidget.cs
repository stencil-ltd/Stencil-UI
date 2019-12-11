using UnityEngine;

namespace Stencil.Ui.Aggregate
{
    public abstract class AggregateWidget : MonoBehaviour
    {
        public GameObject[] aggregate;
        public bool invert;

        public bool ShouldEnable()
        {
            var active = true;
            foreach (var o in aggregate) 
                active &= o.activeSelf;
            return active ^ invert;
        }
        
    }
}