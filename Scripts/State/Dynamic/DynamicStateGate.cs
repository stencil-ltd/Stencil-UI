using System;
using System.Linq;
using State.Active;

namespace State.Dynamic
{
    public class DynamicStateGate : ActiveGate
    {
        public DynamicStateMachine Machine;

        public DynamicState[] States;
        public bool Invert;
        public bool AndDestroy;

        public DynamicState State => Machine.State;

        private void Start()
        {
            foreach (var s in States)
                Machine.Validate(s);
        }

        public override void Register(ActiveManager manager)
        {
            base.Register(manager);
            Machine.OnChange += Changed;
        }

        public override void Unregister()
        {
            Machine.OnChange -= Changed;
        }

        public override bool? Check()
        {
            try 
            {
                var visible = States.Contains(State);
                if (Invert) visible = !visible;
                if (AndDestroy && !visible)
                    Destroy(gameObject);
                return visible;
            } catch (Exception) 
            {
                return null;
            }
        }

        private void Changed(object sender, DynamicStateChange e)
        {
            ActiveManager.Check();
        }
    }
}