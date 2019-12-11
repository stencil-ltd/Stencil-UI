using UnityEngine;

namespace Stencil.Ui.Aggregate
{
    public class AggregateSwitch : AggregateWidget
    {
        public GameObject on;
        public GameObject off;

        private void Update()
        {
            var active = ShouldEnable();
            on.SetActive(active);
            off.SetActive(!active);
        }
    }
}