using System;
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
        public bool rotateInstead;
        
        private StateMachine<T> _machine;

        private void Awake()
        {
            _machine = StateMachines.Get<T>();
            GetComponent<Button>().onClick.AddListener(() =>
            {
                if (popStateInstead)
                {
                    _machine.PopState();
                }
                else if (rotateInstead)
                {
                    var all = (T[]) Enum.GetValues(typeof(T));
                    var current = (int) (object) _machine.State;
                    current = (current + 1) % all.Length;
                    _machine.RequestState((T) (object) current, replaceHistory: replaceHistory);
                }
                else
                {
                    _machine.RequestState(state, replaceHistory: replaceHistory);
                }
                    
            });
        }
    }
}