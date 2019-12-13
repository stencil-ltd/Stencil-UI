using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.Data;
using Plugins.Util;
using Scripts.Prefs;
using UniRx.Async;
using UnityEngine;
using Util;

#if STENCIL_ANALYTICS
using Analytics;
#endif

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

        public string PersistenceKey;
        public bool KeepHistory = true;
        public bool RespectResetButton = false;
        
        public event EventHandler<StateChange<T>> OnChange;

        [Header("Debug")]
        public List<T> History = new List<T>();

        [NonSerialized]
        private int _locked;

        private T? PersistedState
        {
            get
            {
                var idx = StencilPrefs.Default.GetInt(PersistenceKey, -1);
                return (T?) (idx >= 0 ? (object) idx : null);
            }
            set => StencilPrefs.Default.SetInt(PersistenceKey, value != null ? (int) (object) value.Value : -1);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            History.Clear();
            State = GetInitialState(false);
            _locked = 0;
            StateMachines.Register(this);
        }

        protected override void OnFirstLoad()
        {
            base.OnFirstLoad();
            ResetButton.OnGlobalReset += OnResetButton;
            if (Application.isPlaying)
                ResetState(false);
        }

        private void OnDestroy()
        {
            ResetButton.OnGlobalReset -= OnResetButton;
        }

        private void OnResetButton(object sender, EventArgs args)
        {
            if (RespectResetButton)
                ResetState(true);
        }

        public T? PopState()
        {
            if (!KeepHistory) throw new Exception("Need history enabled!");
            if (_locked > 0) return null;
            var index = History.Count - 1;
            if (index < 0) return null;
            var retval = History[index];
            History.RemoveAt(index);
            var ping = History.Count > 0 ? History.Last() : InitialState;
            _SetState(ping);
            return retval;
        }

        private T GetInitialState(bool clearPersistence = true)
        {
            var state = InitialState;
            if (!clearPersistence && !string.IsNullOrEmpty(PersistenceKey))
                state = PersistedState ?? InitialState;
            return state;
        }

        public void ResetState(bool clearPersistence = true)
        {
            if (_locked > 0) return;
            var color = Color;
            Debug.Log($"<color={color.LogString()}>{GetType().ShortName()}</color>: Reset");
            History.Clear();
            RequestState(GetInitialState(clearPersistence), true);
        }

        public void Click_RequestState(T state)
        {
            RequestState(state);
        }

        public void RequestState(T state, bool force = false, bool notify = true, bool replaceHistory = false)
        {
            if (!force && state.Equals(State)) return;
            if (!force && _locked > 0) return;
            if (KeepHistory)
            {
                if (replaceHistory && History.Count > 0)
                    History.RemoveAt(History.Count - 1);
                History.Add(state);
            }
            _SetState(state, notify);
        }

        public void Lock()
        {
            _locked++;
        }

        public void Unlock()
        {
            --_locked;
        }
        
        private async void _SetState(T state, bool notify = true)
        {
            var old = State;
            State = state;
            if (!string.IsNullOrEmpty(PersistenceKey))
                PersistedState = State;
            await UniTask.SwitchToMainThread();
            NotifyChanged(old);
        }

        void NotifyChanged(T old)
        {
            var color = Color;
            var name = ToString();
            Debug.Log($"<color={color.LogString()}>{name} -></color> {State}");
            #if STENCIL_ANALYTICS
            Tracking.Instance
                .Track($"state_change_{name.ToLower()}", "old", old.ToString(), "new", State.ToString());
#endif
            OnChange?.Invoke(this, new StateChange<T>(old, State));
        }

        public override string ToString()
        {
            return GetType().ShortName();
        }
    }
}