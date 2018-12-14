using UnityEngine;

namespace State
{
    public abstract class StateRequester<T> : MonoBehaviour where T : struct
    {
        public T state;

        private void OnEnable()
        {
            StateMachines.Get<T>().RequestState(state);
        }
    }
}