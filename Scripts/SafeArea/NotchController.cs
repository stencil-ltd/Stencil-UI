using Binding;
using Dev;
using Scripts.RemoteConfig;
using UI;
using UnityEngine;

namespace Stencil.UI.SafeArea
{
    public class NotchController : Controller<NotchController>
    {
        public static Rect SafeArea => Screen.safeArea;
        
        [Header("Debug")] 
        public bool debugNotch;
        public bool yesEvenOnDevice;
        public float debugNotchTop = 132;
        public float debugNotchBottom = 102;

        [RemoteField("notch_ignore_scale")] 
        public bool IgnoreCanvasScale { get; } = false;

        public float TopSafePadding { get; private set; }
        public float BottomSafePadding { get; private set; }
        
        protected override void OnAwake()
        {
            base.OnAwake();
            this.BindRemoteConfig();
            var safe = SafeArea;
            TopSafePadding = Screen.height - safe.yMax;
            BottomSafePadding = safe.yMin;
            if (debugNotch && (yesEvenOnDevice || Application.isEditor) && StencilRemote.IsDeveloper())
            {
                if (debugNotchTop >= 1f)
                    TopSafePadding = debugNotchTop;
                if (debugNotchBottom >= 1f)
                    BottomSafePadding = debugNotchBottom;
            }
        }
    }
}