using System;
using System.Collections.Generic;
using Plugins.Util;
using UnityEngine;
using Util;

namespace State
{
    public static class StateMachines
    {
        private static Dictionary<Type, IStateMachine> _instances 
            = new Dictionary<Type, IStateMachine>();

        internal static void Register<T>(StateMachine<T> machine) where T : struct
        {
            _instances[typeof(T)] = machine;
        }
        
        internal static void Unregister<T>(StateMachine<T> machine) where T : struct
        {
            IStateMachine current;
            _instances.TryGetValue(typeof(T), out current);
            if (current == (object) machine)
                _instances.Remove(typeof(T));
        }
        
        public static StateMachine<T> Get<T>() where T : struct
        {
            return _instances[typeof(T)] as StateMachine<T>;
        }

        public static void Initialize()
        {
            if (!Application.isPlaying) return;
            foreach (var m in _instances.Values)
                m.ResetState();
        }
    }
    
    public abstract class StateMachine<T> : Singleton<StateMachine<T>>, IStateMachine where T : struct
    {
        public Color Color;

        public T InitialState;
        public T State;
    
        public readonly string Name = typeof(T).ShortName();
    
        public event EventHandler<StateChange<T>> OnChange;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (!typeof(T).IsEnum)
                throw new Exception("StateMachine can only handle enums.");
            StateMachines.Register(this);
        }

        protected override void OnFirstLoad()
        {
            base.OnFirstLoad();
            if (Application.isPlaying)
                ResetState();
        }

        public void ResetState()
        {
            var color = Color;
            Debug.Log($"<color={color.LogString()}>{GetType().ShortName()}</color>: Reset");
            RequestState(InitialState, true);
        }

        public void Click_RequestState(T state)
        {
            RequestState(state);
        }

        public void RequestState(T state, bool force = false, bool notify = true)
        {
            if (!force && state.Equals(State)) return;
            var old = State;
            State = state;
            if (notify) Objects.OnMain(() => NotifyChanged(old)); 
        }

        void NotifyChanged(T old)
        {
            var color = Color;
            Debug.Log($"<color={color.LogString()}>{GetType().ShortName()} -></color> {State}");
            OnChange?.Invoke(this, new StateChange<T>(old, State));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}