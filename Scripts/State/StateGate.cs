using System;
using System.Linq;
using State.Active;

namespace State
{
    public abstract class StateGate<StateType> : ActiveGate where StateType : struct
    {
        public bool Invert;
        public bool AndDestroy;
        public bool RevertOnExit;
        public bool TakeStateOnActive;
        public StateType[] States;

        public StateMachine<StateType> Machine;
        public StateType State => Machine.State;

        private StateType? RevertState;

        public override void Register(ActiveManager manager)
        {
            base.Register(manager);
            if (Machine == null)
                Machine = StateMachines.Get<StateType>();
            Machine.OnChange += Changed;
        }

        public override void Unregister()
        {
            Machine.OnChange -= Changed;
        }

        private void OnEnable()
        {
            if (TakeStateOnActive)
                Machine.RequestState(States[0]);
        }

        private void OnDisable()
        {
            if (RevertOnExit && RevertState != null)
            {
                Machine.RequestState(RevertState.Value);   
                RevertState = null;
            }
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

        private void Changed(object sender, StateChange<StateType> e)
        {
            if (RevertOnExit && e.Old != null && !States.Contains(e.Old.Value))
                RevertState = e.Old.Value;
            if (ActiveManager != null) ActiveManager.Check();
        }
    }
}