using UnityEngine;

namespace State
{
    public abstract class StateRequester<T> : MonoBehaviour where T : struct
    {
        public bool dormant;
        public T state;

        private void OnEnable()
        {
            if (!dormant) 
                Invoke(nameof(_Check), 0.1f);
        }

        private void _Check()
        {
            if (gameObject.activeInHierarchy)
                RequestState();
        }

        public void RequestState()
        {
            StateMachines.Get<T>().RequestState(state);
        }
    }
}