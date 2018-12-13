using System;
using System.Collections.Generic;
using System.Linq;
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
        public readonly List<T> History = new List<T>();
        
        public Color Color;

        public T InitialState;
        public T State;

        public bool KeepHistory;
        public readonly string Name = typeof(T).ShortName();
    
        public event EventHandler<StateChange<T>> OnChange;
        
        protected override void OnEnable()
        {
            base.OnEnable();
//            if (!typeof(T).IsEnum)
//                throw new Exception("StateMachine can only handle enums.");
            StateMachines.Register(this);
        }

        protected override void OnFirstLoad()
        {
            base.OnFirstLoad();
            if (Application.isPlaying)
                ResetState();
        }

        public T? PopState()
        {
            if (!KeepHistory) throw new Exception("Need history enabled!");
            var index = History.Count - 1;
            if (index < 0) return null;
            var retval = History[index];
            History.RemoveAt(index);
            var ping = History.Count > 0 ? History.Last() : InitialState;
            _SetState(ping);
            return retval;
        }

        public void ResetState()
        {
            var color = Color;
            Debug.Log($"<color={color.LogString()}>{GetType().ShortName()}</color>: Reset");
            RequestState(InitialState, true);
            History.Clear();
        }

        public void Click_RequestState(T state)
        {
            RequestState(state);
        }

        public void RequestState(T state, bool force = false, bool notify = true, bool replaceHistory = false)
        {
            if (!force && state.Equals(State)) return;
            if (KeepHistory)
            {
                if (replaceHistory && History.Count > 0)
                    History.RemoveAt(History.Count - 1);
                History.Add(state);
            }
            _SetState(state, notify);
        }

        private void _SetState(T state, bool notify = true)
        {
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