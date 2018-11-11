using Store;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace State
{
    public abstract class StateClick<StateType> : MonoBehaviour, IPointerClickHandler where StateType : struct
    {
        public StateType State;

        public bool RevertOnSecondClick;
        public StateType RevertTo;
        
        private StateMachine<StateType> _machine;

        private void Awake()
        {
            _machine = StateMachines.Get<StateType>();
            var btn = GetComponent<Button>();
            btn?.onClick.AddListener(Execute);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Execute();
        }

        private void Execute()
        {
            if (RevertOnSecondClick && _machine.State.Equals(State))
                _machine.RequestState(RevertTo);
            else 
                _machine.RequestState(State);
        }
    }
}