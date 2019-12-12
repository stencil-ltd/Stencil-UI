using System;
using Binding;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Stencil.Ui.Widgets
{
    [RequireComponent(typeof(Button))]
    public class Checkbox : RegisterableBehaviour
    {
        public GameObject check;

        [Bind]
        public Button button { get; private set; }

        public bool IsChecked
        {
            get => check.activeSelf;
            set => check.SetActive(value);
        }

        public override void Register()
        {
            base.Register();
            this.Bind();
        }
    }
}