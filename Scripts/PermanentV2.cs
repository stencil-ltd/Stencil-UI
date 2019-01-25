using UnityEngine;

namespace UI
{
    public abstract class PermanentV2<T> : RegisterableBehaviour where T : PermanentV2<T>
    {
        public static T Instance { get; private set; }
        protected bool Valid;

        private void Awake()
        {
            AwakeCheck();
            if (Valid) OnAwake();
        }

        private void Start()
        {
            if (Valid) OnStart();
        }

        public override void Register()
        {
            AwakeCheck();
        }

        private bool AwakeCheck()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return false;
            }
            Valid = true;
            Instance = (T) this;
            DontDestroyOnLoad(gameObject);
            return true;
        }
        
        protected virtual void OnAwake() {}
        protected virtual void OnStart() {}
        
        protected virtual void OnDestroy()
        {
            Instance = Instance != this ? Instance : null;
        }
    }
}