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
            #if UNITY_EDITOR || !UNITY_ANDROID
            return;
            #endif
            var build = new AndroidJavaClass("android.os.Build");
            var mfg = build.GetStatic<string>("MANUFACTURER").ToLower().Trim();
            if (mfg == "google")
            {
                var rect = image.rectTransform;
                var scale = rect.localScale;
                scale.y = -scale.y;
                rect.localScale = scale;
            }
        }
    }
}