using System;
using Ads.Admob;
using Binding;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Developers = Dev.Developers;

namespace Plugins.UI
{
    public class Frame : MonoBehaviour
    {
        public static Frame Instance;
        
        public static Rect SafeArea => Screen.safeArea;

        [CanBeNull] 
        public Action OnClick;
        
        [CanBeNull] public Mask GraphicMask;
        
        public RectTransform Contents;
        [CanBeNull] public RectTransform Scrim;
        
        [Tooltip("This frame will auto-adjust to the ad area.")]
        public bool AutoAdZone = true;

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

        private float _bannerHeight;

        private void Awake()
        {
            Instance = this;
            this.Bind();
            eventSystem = EventSystem.current;
            var safe = SafeArea;
            TopSafePadding = Screen.height - safe.yMax;
            BottomSafePadding = safe.yMin;
            if (Developers.Enabled && DebugNotch)
            {
                if (DebugNotchTop >= 1f)
                    TopSafePadding = DebugNotchTop;
                if (DebugNotchBottom >= 1f)
                    BottomSafePadding = DebugNotchBottom;
            }
        }

        void Start()
        {
            if (_bannerHeight <= 1f) SetBannerHeight(0f, true);
            AdmobBannerArea.OnChange += OnBanner;
            OnBanner(null, null);
            
            if (GraphicMask != null)
            {
                GraphicMask.enabled = true;
                GraphicMask.GetComponent<Image>().enabled = true;
            }
        }

        private void OnBanner(object sender, EventArgs eventArgs)
        {
            SetBannerHeight(AdmobBannerArea.BannerHeight, AdmobBannerArea.IsTop);
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
            if (Instance == this) Instance = null;
            AdmobBannerArea.OnChange -= OnBanner;
        }

        public void Lock()
        {
            lockCount++;
            _SetLocked(true);
        }

        private void SetScrim(bool top)
        {
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
            if (!top) hBot += _bannerHeight;
            Debug.Log($"Set Contents {hTop}x{hBot}");
            Contents.offsetMax = new Vector2(0, -hTop);
            Contents.offsetMin = new Vector2(0, hBot);
        }

        public void SetBannerHeight(float pixelHeight, bool top)
        {
            var scaler = GetComponentInParent<CanvasScaler>();
            if (scaler == null) return;
            var ratio = scaler.referenceResolution.x / Screen.width;
            _bannerHeight = pixelHeight * ratio;
            SetScrim(top);
            SetContents(top);
            Debug.Log($"Setting banner height to {_bannerHeight}");
            Scrim?.gameObject.SetActive(pixelHeight >= 1f);
        }

        private void _SetLocked(bool locked)
        {
            eventSystem.enabled = !locked;
        }

        public void Unlock()
        {
            if (--lockCount == 0) _SetLocked(false);
        }
    }
}