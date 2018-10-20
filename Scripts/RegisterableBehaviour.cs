using UnityEngine;

namespace UI
{
    public abstract class RegisterableBehaviour : MonoBehaviour
    {
        public bool Registered { get; internal set; }
        public bool Unregistered { get; internal set; }
        
        public virtual void Register() {}
        public virtual void DidRegister() {}
        
        public virtual void Unregister() {}        
        public virtual void WillUnregister() {}
    }
}
