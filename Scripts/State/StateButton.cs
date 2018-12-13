using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace State
{
    [RequireComponent(typeof(Button))]
    public abstract class StateButton<T> : MonoBehaviour where T : struct
    {
        [Header("Config")] 
        public T state;
        public bool popStateInstead;
        public bool replaceHistory;
        
        private StateMachine<T> _machine;

        private void Awake()
        {
            _machine = StateMachines.Get<T>();
            GetComponent<Button>().onClick.AddListener(() =>
            {
                if (popStateInstead)
                    _machine.PopState();
                else 
                    _machine.RequestState(state, replaceHistory: replaceHistory);
            });
        }
    }
}