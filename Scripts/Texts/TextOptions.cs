using System;
using Plugins.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Texts
{
    [RequireComponent(typeof(Text))]
    public class TextOptions : MonoBehaviour
    {
        public bool refreshOnEnabled = true;
        public string[] options = {};

        private void OnEnable()
        {
            if (refreshOnEnabled)
                Refresh();
        }

        public void Refresh()
        {
            GetComponent<Text>().text = options.Random();
        }
    }
}