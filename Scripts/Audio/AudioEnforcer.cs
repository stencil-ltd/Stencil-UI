using UI;

namespace Scripts.Audio
{
    public class AudioEnforcer : RegisterableBehaviour
    {
        public override void DidRegister()
        {
            base.DidRegister();
            AudioSystem2.Instance.UpdateMixers();
        }

        private void Awake()
        {
            AudioSystem2.Instance.UpdateMixers();
        }

        private void Start()
        {
            AudioSystem2.Instance.UpdateMixers();
        }
    }
}