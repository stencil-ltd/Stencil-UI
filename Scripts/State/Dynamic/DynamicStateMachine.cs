using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Plugins.Util;
using UnityEngine;
using Util;

namespace State.Dynamic
{
    [CreateAssetMenu(menuName = "Dynamic States/Machine")]
    public class DynamicStateMachine : Singleton<DynamicStateMachine>
    {
        public string Name;
        public Color Color;

        [NotNull] public DynamicState DefaultState;

        public DynamicState[] ValidStates;
        HashSet<DynamicState> _valid;

        [NotNull] public DynamicState State;
        public event EventHandler<DynamicStateChange> OnChange;

        private void Awake()
        {
            if (Application.isPlaying)
                Reset();
        }

        public void Reset()
        {
            _valid = ValidStates == null ? new HashSet<DynamicState>() : new HashSet<DynamicState>(ValidStates);
            RequestState(DefaultState, true, true);
        }

        public void Click_RequestState(DynamicState state)
        {
            RequestState(state);
        }

        public void RequestState([NotNull] DynamicState state, bool force = false, bool notify = true)
        {
            if (!force && state.Equals(State)) return;
            Validate(state);
            var old = State;
            State = state;
            if (notify) Objects.OnMain(() => NotifyChanged(old)); 
        }

        public void Validate(DynamicState state) 
        {
            if (state == null) throw new Exception("Default state cannot be null. Create a null instance if you want.");
            if (!_valid.Contains(state)) throw new Exception($"Don't recognize {state}"); 
        }

        void NotifyChanged(DynamicState old)
        {
            var color = Color;
            Debug.Log($"<color={color.LogString()}>{Name} -> {State.Name}</color>");
            OnChange?.Invoke(this, new DynamicStateChange(old, State));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}