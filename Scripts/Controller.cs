namespace UI
{
    public abstract class Controller<T> : RegisterableBehaviour where T : Controller<T>
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            Instance = (T)this;
        }

        private void OnEnable()
        {
            Instance = (T)this;
        }

        public override void Register()
        {
            Instance = (T)this;
        }

        public override void Unregister()
        {
            Instance = Instance == this ? null : Instance;
        }
    }
}