using Binding;
using Dev;
using UI;
using UnityEngine;

namespace Stencil.UI.SafeArea
{
    public class NotchRoot : Controller<NotchRoot>
    {
        public static Rect SafeArea => Screen.safeArea;
        
        [Header("Debug")] 
        public bool DebugNotch;
        public float DebugNotchTop = 132;
        public float DebugNotchBottom = 102;
        
        public float TopSafePadding { get; private set; }
        public float BottomSafePadding { get; private set; }
        
        protected override void OnAwake()
        {
            base.OnAwake();
            this.Bind();
            var safe = SafeArea;
            TopSafePadding = Screen.height - safe.yMax;
            BottomSafePadding = safe.yMin;
            if (Application.isEditor && Developers.Enabled && DebugNotch)
            {
                if (DebugNotchTop >= 1f)
                    TopSafePadding = DebugNotchTop;
                if (DebugNotchBottom >= 1f)
                    BottomSafePadding = DebugNotchBottom;
            }
        }
    }
}