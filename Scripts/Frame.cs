using System;
using Binding;
using JetBrains.Annotations;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Developers = Dev.Developers;

namespace Plugins.UI
{
    public class Frame : Controller<Frame>
    {
        public static Rect SafeArea => Screen.safeArea;

        [CanBeNull] 
        public Action OnClick;
        
        [CanBeNull] public Mask GraphicMask;
        
        public RectTransform Contents;
        
        [Tooltip("This frame will auto-adjust to the ad area.")]
        public bool AutoAdZone = true;
        public float BannerNudge = 20f;

        [Header("Debug")] 
        public bool DebugNotch;
        public float DebugNotchTop = 132;
        public float DebugNotchBottom = 102;
        
        public float TopSafePadding { get; private set; }
        public float BottomSafePadding { get; private set; }

        [Bind]
        private BoxCollider2D _collider;
        private int lockCount;
        private EventSystem eventSystem;

        private CanvasScaler _scaler;
        private Canvas _canvas;

        private float SizeFactor => _scaler.referenceResolution.x / Screen.width;
        private float FinalScale => SizeFactor;

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
            
            _scaler = GetComponentInParent<CanvasScaler>();
            _canvas = _scaler.GetComponent<Canvas>();
        }

        void Start()
        {
            eventSystem = EventSystem.current;
            if (GraphicMask != null)
            {
                GraphicMask.enabled = true;
                GraphicMask.GetComponent<Image>().enabled = true;
            }
        }

        void OnMouseUpAsButton()
        {
            OnClick?.Invoke();
        }

        void Update()
        {
            if (_collider != null) _collider.enabled = OnClick != null;
        }

        public void Lock()
        {
            lockCount++;
            _SetLocked(true);
        }

        private void SetContents()
        {
            var hTop = TopSafePadding;
            var hBot = BottomSafePadding;
            Debug.Log($"Set Contents {hTop}x{hBot}");
            Contents.offsetMax = new Vector2(0, -hTop);
            Contents.offsetMin = new Vector2(0, hBot);
        }
        
        private void _SetLocked(bool locked)
        {
            if (eventSystem == null) eventSystem = EventSystem.current;
            eventSystem.enabled = !locked;
        }

        public void Unlock()
        {
            if (--lockCount == 0) _SetLocked(false);
        }

        public void ResetLock()
        {
            lockCount = 0;
            _SetLocked(false);
        }
    }
}