using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Audio.Ng
{
    [RequireComponent(typeof(Button))]
    public class SmartSfxButton : MonoBehaviour
    {
        public SmartSfx sfx;
        
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(call: () =>
            {
                if (sfx != null)
                    sfx.Play();
            });
        }
    }
}