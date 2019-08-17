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

        [Range(0, 1)] 
        public float disabledAlpha = 1f;
        
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

        private void OnEnable()
        {
            _machine.OnChange += _OnState;
            _Refresh();
        }

        private void OnDisable()
        {
            _machine.OnChange -= _OnState;
        }

        private void _OnState(object sender, StateChange<T> e)
        {
            _Refresh();
        }

        private void _Refresh()
        {
            var cg = GetComponent<CanvasGroup>();
            if (cg != null)
            {
                var alpha = _machine.State.Equals(state) ? 1f : disabledAlpha;
                cg.alpha = alpha;
            }
        }
    }
}