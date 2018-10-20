using UnityEngine;

namespace State.Active
{
    [RequireComponent(typeof(ActiveManager))]
    public abstract class ActiveGate : MonoBehaviour
    {
        public ActiveManager ActiveManager { get; private set; }

        public abstract bool? Check();
        public virtual void Register(ActiveManager manager) 
        {
            ActiveManager = manager;
        }
        public virtual void Unregister() {}
    }
}