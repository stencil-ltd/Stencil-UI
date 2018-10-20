using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace State.Dynamic
{
    [Serializable]
    public class DynamicStateListenEvent : UnityEvent<DynamicStateChange>
    {}

    public class DynamicStateListener : MonoBehaviour
    {
        public DynamicStateMachine StateMachine;
        public DynamicState[] States;
        public DynamicStateListenEvent OnState;

        private void Awake()
        {
            StateMachine.OnChange += OnChange;
        }

        private void OnDestroy()
        {
            StateMachine.OnChange -= OnChange;
        }

        private void OnChange(object sender, DynamicStateChange e)
        {
            if (States.Contains(e.New))
                OnState.Invoke(e);
        }
    }
}