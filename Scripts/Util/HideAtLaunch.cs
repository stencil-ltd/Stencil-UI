using UI;

namespace Init
{
    public class HideAtLaunch : RegisterableBehaviour
    {
        public override void Register()
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }

        public override void Unregister()
        {}
    }
}