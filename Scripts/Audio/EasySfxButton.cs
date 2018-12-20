using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Standard.Audio
{
    [RequireComponent(typeof(Button))]
    public class EasySfxButton : MonoBehaviour
    {
        public EasySound sfx;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => sfx.Play());
        }
    }
}