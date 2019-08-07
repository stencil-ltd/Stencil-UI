using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Texture
{
    public static class TextureExtensions
    {
        public static Sprite ToSprite([CanBeNull] this Texture2D tex)
        {
            if (tex == null) return null;
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }

        public static void SetTexture(this Image image, Texture2D tex)
        {
            image.sprite = tex.ToSprite();
        }
    }
}