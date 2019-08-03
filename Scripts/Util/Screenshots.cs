using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Util.UI
{
    public static class Screenshots
    {
        public static IEnumerator TakeScreenshot(this RectTransform transform, Action<Texture2D> callback)
        {
            var screenRect = transform.ToScreenSpace();
            var tex = new Texture2D((int) screenRect.width, (int) screenRect.height, TextureFormat.ARGB32, false);
            yield return new WaitForEndOfFrame();
            tex.ReadPixels(screenRect, 0, 0);
            tex.Apply();
            callback.Invoke(tex);
            yield return null;
        }

        public static IEnumerator ShareScreenshotSimple([CanBeNull] string title = null)
        {
            yield return new WaitForEndOfFrame();
            var path = "screenshot.png";
            ScreenCapture.CaptureScreenshot(path);
            #if !UNITY_EDITOR && STENCIL_NATIVE_SHARE
            new NativeShare()
                .SetTitle(title)
                .AddFile($"{Application.persistentDataPath}/{path}")
                .Share();
            #endif
        }
    }
}