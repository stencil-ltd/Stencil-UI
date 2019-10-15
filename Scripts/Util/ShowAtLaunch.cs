using UI;

namespace Scripts.Util
{
    public class ShowAtLaunch : RegisterableBehaviour
    {
        public override void Register()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
        }

        public override void Unregister()
        {}
    }
}