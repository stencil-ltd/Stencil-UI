using UnityEngine;

namespace State
{
    public abstract class StateRequester<T> : MonoBehaviour where T : struct
    {
        public T state;

        private void OnEnable()
        {
            Invoke(nameof(_Check), 0.1f);
        }

        private void _Check()
        {
            if (gameObject.activeInHierarchy)
                StateMachines.Get<T>().RequestState(state);            
        }
    }
}