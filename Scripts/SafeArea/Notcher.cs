using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Stencil.UI.SafeArea
{
    public class Notcher : MonoBehaviour
    {
        public bool undoNotch;
        
        public float topScale = 1f;
        public float bottomScale = 1f;
        
        [Bind] 
        private RectTransform _rect;

        private bool _updated;
        private Canvas _canvas;
        
        private void Awake()
        {
            this.Bind();
            _canvas = GetComponentInParent<Canvas>();
        }
        
        private void Start()
        {
            LogDiagnostics();
            ApplyOffset();
        }

        private void ApplyOffset()
        {
            if (_updated) return;
            _updated = true;
            var scale = NotchController.Instance.IgnoreCanvasScale ? 1f : _canvas.scaleFactor;
            var top = NotchController.Instance.TopSafePadding * topScale / scale;
            var bot = NotchController.Instance.BottomSafePadding * bottomScale / scale;

            var min = _rect.anchorMin;
            var max = _rect.anchorMax;

            if (!undoNotch)
            {
                top *= -1;
                bot *= -1;
            }

            if (min.y == 0)
            {
                Debug.Log("Ignore Safe Area Min");
                var offset = _rect.offsetMin;
                offset.y -= bot;
                _rect.offsetMin = offset;
            }
            else if (min.y == 1)
            {
                var offset = _rect.offsetMin;
                offset.y += top;
                _rect.offsetMin = offset;
            }

            if (max.y == 0)
            {
                Debug.Log("Ignore Safe Area Max");
                var offset = _rect.offsetMax;
                offset.y -= bot;
                _rect.offsetMax = offset;
            }
            else if (max.y == 1)
            {
                Debug.Log("Ignore Safe Area Max");
                var offset = _rect.offsetMax;
                offset.y += top;
                _rect.offsetMax = offset;
            }
        }

        private void LogDiagnostics()
        {
            var canvas = _canvas;
            var scaler = canvas.GetComponent<CanvasScaler>();
            var ppu = canvas.referencePixelsPerUnit;
            var ppu2 = scaler.referencePixelsPerUnit;
            var dppu = scaler.dynamicPixelsPerUnit;
            var rect = canvas.pixelRect;
            var scale = canvas.scaleFactor;
            var scale2 = scaler.scaleFactor;
            var sdpi = scaler.defaultSpriteDPI;
            var fsdpi = scaler.fallbackScreenDPI;
            var ct = NotchController.Instance;
            Debug.Log($"{name} Canvas Info: ( " +
                      $"Screen={Screen.currentResolution}, " +
                      $"NotchController=({ct.TopSafePadding}x{ct.BottomSafePadding}), " +
                      $"SafeArea={Screen.safeArea}, " +
                      $"Lossy Scale={_rect.lossyScale}, " +
                      $"ppu={ppu}, " +
                      $"ppu2={ppu2}, " +
                      $"dppu={dppu}, " +
                      $"rect={rect}, " +
                      $"scale={scale}, " +
                      $"scale2={scale2}, " +
                      $"sdpi={sdpi}, " +
                      $"fsdpi={fsdpi}, " +
                      $")");}
    }
}