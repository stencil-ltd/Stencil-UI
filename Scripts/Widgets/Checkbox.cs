using System;
using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Stencil.Ui.Widgets
{
    [RequireComponent(typeof(Button))]
    public class Checkbox : MonoBehaviour
    {
        public GameObject check;

        [Bind]
        private Button _button;

        public bool IsChecked
        {
            get => check.activeSelf;
            set => check.SetActive(value);
        }

        private void Awake()
        {
            this.Bind();
        }
    }
}