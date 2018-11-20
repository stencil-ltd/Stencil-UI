using UnityEngine;

namespace UI
{
    public class EnabledController<T> : MonoBehaviour where T : EnabledController<T>
    {
        public static T Instance { get; private set; }
        
        private void OnEnable()
        {
            Instance = (T)this;
        }

        private void OnDisable()
        {
            Instance = Instance == this ? null : Instance;
        }
    }
}