using System;
using UI;
using UnityEngine;

namespace UI.Performance
{
    public class StencilFramerate : Controller<StencilFramerate>
    {
        private static int _count = 0;

        public static void Acquire() => _count++;
        public static void Release() => _count--;
        public static bool UseFullRate() => _count > 0;

        private void Update()
        {
            Application.targetFrameRate = UseFullRate() ? 60 : 30;
        }
    }
}