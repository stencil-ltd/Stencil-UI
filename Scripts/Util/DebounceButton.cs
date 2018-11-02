using System.Collections;
using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Util
{
    [RequireComponent(typeof(Button))]
    public class DebounceButton : MonoBehaviour
    {
        public float Delay = 0.5f;
        
        [Bind] private Button _button;

        private void Awake()
        {
            this.Bind();
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            Objects.StartCoroutine(_OnClick());
        }

        private IEnumerator _OnClick()
        {
            _button.enabled = false;
            yield return new WaitForSeconds(Delay);
            _button.enabled = true;
        }
    }
}