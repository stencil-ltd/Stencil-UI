using System;
using UnityEngine;

namespace State.Containers
{
    public abstract class StateContainer<T> : StencilData, IStateHaver<T>
    {
        public const string CreateFolder = "State Containers/";
        
        public event EventHandler<StateTransition<T>> OnChange;

        [SerializeField] 
        private T _value;

        public T Value
        {
            get { return _value; }
            set
            {
                if (Equals(_value, value)) return;
                var old = _value;
                _value = value;
                OnChange?.Invoke(this, new StateTransition<T>(old, _value));
            }
        }

        protected virtual bool Equals(T oldValue, T newValue)
        {
            return oldValue?.Equals(newValue) == true;
        }
    }
}