using System;
using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Stencil.Ui.Aggregate
{
    [RequireComponent(typeof(Button))]
    public class AggregateActiveButton : AggregateWidget
    {

        [Bind] private Button _button;

        private void Awake()
        {
            this.Bind();
        }

        private void Update()
        {
            _button.interactable = ShouldEnable();
        }
    }
}