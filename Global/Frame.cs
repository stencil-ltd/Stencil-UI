using System;
using Binding;
using JetBrains.Annotations;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Developers = Dev.Developers;

#if STENCIL_ADS
using Ads;
#endif

namespace Plugins.UI
{
    public class Frame : Controller<Frame>
    {
        public static Rect SafeArea => Screen.safeArea;

        [CanBeNull] 
        public Action OnClick;
        
        [CanBeNull] public Mask GraphicMask;
        
        public RectTransform Contents;
        [CanBeNull] public RectTransform Scrim;
        
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
        
        private float _bannerHeight;

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

            #if STENCIL_ADS
            if (BottomSafePadding >= 1f && AdSettings.Instance != null)
                BottomSafePadding += AdSettings.Instance.NudgeBottomSafeZone;
            StencilAds.OnBannerChange += OnBanner;
            #endif
            
            _scaler = GetComponentInParent<CanvasScaler>();
            _canvas = _scaler.GetComponent<Canvas>();
        }

        void Start()
        {
            eventSystem = EventSystem.current;
            if (_bannerHeight <= 1f) SetBannerHeight(0f, true);
            OnBanner(null, null);
            
            if (GraphicMask != null)
            {
                GraphicMask.enabled = true;
                GraphicMask.GetComponent<Image>().enabled = true;
            }
        }

        private void OnBanner(object sender, EventArgs eventArgs)
        {
#if STENCIL_ADS
            SetBannerHeight(StencilAds.BannerHeight, false);
#else
            SetBannerHeight(0, false);
#endif
        }

        void OnMouseUpAsButton()
        {
            OnClick?.Invoke();
        }

        void Update()
        {
            if (_collider != null) _collider.enabled = OnClick != null;
        }

        void OnDestroy()
        {
#if STENCIL_ADS
            StencilAds.OnBannerChange -= OnBanner;
#endif
        }

        public void Lock()
        {
            lockCount++;
            _SetLocked(true);
        }

        private void SetScrim(bool top)
        {
            if (Scrim == null) return;
            var height = _bannerHeight;
            height += top ? TopSafePadding : BottomSafePadding;
            Scrim?.SetInsetAndSizeFromParentEdge(top ? RectTransform.Edge.Top : RectTransform.Edge.Bottom, 0, height);
            Debug.Log($"Set Scrim {height}");
        }

        private void SetContents(bool top)
        {
            var hTop = TopSafePadding;
            if (top) hTop += _bannerHeight;
            var hBot = BottomSafePadding;
            if (!top) hBot += _bannerHeight == 0 ? 0 : _bannerHeight + BannerNudge;
            Debug.Log($"Set Contents {hTop}x{hBot}");
            Contents.offsetMax = new Vector2(0, -hTop);
            Contents.offsetMin = new Vector2(0, hBot);
        }

        public void SetBannerHeight(float pixelHeight, bool top)
        {
            if (Scrim != null)
            {
                var ratio = 1f;
                if (Application.isEditor) 
                    ratio = 2f;
                else 
                {
#if STENCIL_ADS
                    if (StencilAds.BannerNeedsScale)
                        ratio = FinalScale;
#endif
                }
                _bannerHeight = pixelHeight * ratio;
                Debug.Log($"Setting banner height to {_bannerHeight} ({pixelHeight} x {ratio:N2})");
                Scrim?.gameObject.SetActive(pixelHeight >= 1f);
            }
            SetScrim(top);
            SetContents(top);
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