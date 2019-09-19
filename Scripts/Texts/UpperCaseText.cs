using System;
using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Texts
{
    [RequireComponent(typeof(Text))]
    public class UpperCaseText : MonoBehaviour
    {
        [Bind] private Text _text;

        private void Awake()
        {
            this.Bind();
        }

        private void LateUpdate()
        {
            _text.text = _text.text.ToUpper();
        }
    }
}