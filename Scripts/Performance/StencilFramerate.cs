using System;
using UI;
using UnityEngine;

namespace UI.Performance
{
    public class StencilFramerate : Controller<StencilFramerate>
    {
        private int _count = 0;

        public void Acquire() => _count++;
        public void Release() => _count--;
        public bool UseFullRate() => _count > 0;

        private void Update()
        {
            Application.targetFrameRate = UseFullRate() ? 60 : 30;
        }
    }
}