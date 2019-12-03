using UnityEngine;
using UnityEngine.UI;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace Scripts.Texture
{
    public static class InvertedTextureHacks
    {
        // Not idempotent. Please only call once. Results not guaranteed.
        public static void InvertedVideoHack(this RawImage image)
        {
            if (ShouldApplyVideoHack())
            {
                var rect = image.rectTransform;
                var scale = rect.localScale;
                scale.y = -scale.y;
                rect.localScale = scale;
            }
        }

        public static void InvertedVideoHack2(this RawImage image, bool mirror = false)
        {
            var rect = new Rect(mirror ? 1 : 0, 0, mirror ? -1 : 1, 1);
            if (ShouldApplyVideoHack())
            {
                rect.y = 1;
                rect.height = -1;
            }
            image.uvRect = rect;
        }
        
        public static void InvertedVideoHack3(this RawImage image, bool mirror = false)
        {
            var scale = image.transform.localScale;
            if (mirror) scale.x *= -1;
            if (ShouldApplyVideoHack()) scale.y *= -1;
            image.transform.localScale = scale;
        }

        public static bool ShouldApplyVideoHack()
        {
            return true;
#if UNITY_EDITOR || !UNITY_ANDROID
            return false;
#endif
            var build = new AndroidJavaClass("android.os.Build");
            var mfg = build.GetStatic<string>("MANUFACTURER").ToLower().Trim();
            return mfg == "google";
        }
    }
}