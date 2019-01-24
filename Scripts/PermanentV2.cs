using UnityEngine;

namespace UI
{
    public abstract class PermanentV2<T> : RegisterableBehaviour where T : PermanentV2<T>
    {
        public static T Instance { get; private set; }
        protected bool Valid;

        private void Awake()
        {
            Instance = (T)this;
            AwakeCheck();
            OnAwake();
        }

        private void OnEnable()
        {
            Instance = (T)this;
        }

        public override void Register()
        {
            Instance = (T)this;
            AwakeCheck();
        }

        public override void Unregister()
        {
            Instance = Instance == this ? null : Instance;
        }

        private void AwakeCheck()
        {
            if (Instance != null && Instance != this && Application.isPlaying)
            {
                Destroy(gameObject);
                return;
            }
            Valid = true;
            Instance = (T) this;
            if (Application.isPlaying) DontDestroyOnLoad(gameObject);
        }
        
        protected virtual void OnAwake() {}
        
        protected virtual void OnDestroy()
        {
            Instance = Instance != this ? Instance : null;
        }
    }
}