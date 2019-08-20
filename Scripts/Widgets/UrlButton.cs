using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Widgets
{
    [RequireComponent(typeof(Button))]
    public class UrlButton : MonoBehaviour
    {
        public string url;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(url));
        }
    }
}