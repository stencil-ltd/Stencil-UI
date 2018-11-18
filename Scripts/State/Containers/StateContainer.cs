using System;
using UnityEngine;
using Util;

namespace State.Containers
{
    public abstract class StateContainer<T> : StencilData, IStateHaver<T>
    {
        public const string CreateFolder = "State Containers/";
        
        public event EventHandler<StateTransition<T>> OnChange;

        [Header("Values")]
        [SerializeField]
        [LabelOverride("Initial Value")]
        private T _initialValue;
        
        [SerializeField] 
        [LabelOverride("Current Value")]
        private T _value;

        private T _previous;

        public T Value
        {
            get { return _value; }
            set
            {
                if (Equals(_value, value)) return;
                _previous = _value;
                _value = value;
                Notify();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _value = _initialValue;
            _previous = _value;
            Notify();
        }

        public void Notify()
        {
            OnChange?.Invoke(this, new StateTransition<T>(_previous, _value));
        }

        protected virtual bool Equals(T oldValue, T newValue)
        {
            return oldValue?.Equals(newValue) == true;
        }
    }
}