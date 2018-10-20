using Binding;
using CustomOrder;
using Plugins.UI;

namespace State
{
    [ExecutionOrder(-50)]
    public class StateMachineObject : Permanent<StateMachineObject>
    {
        protected override void Awake()
        {
            base.Awake();
            this.BindStates();
        }

        private void Start()
        {
            StateMachines.Initialize();
        }
    }
}