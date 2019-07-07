using System;
using UnityEngine;

namespace UI
{
    public static class StencilColor
    {
        // Example: "#ff000099".ToColor() red with alpha ~50%
        // Example: "ffffffff".ToColor() white with alpha 100%
        // Example: "00ff00".ToColor() green with alpha 100%
        // Example: "0000ff00".ToColor() blue with alpha 0%
        public static Color ToColor(this string color)
        {
            if (color.StartsWith("#", StringComparison.InvariantCulture))
            {
                color = color.Substring(1); // strip #
            }

            if (color.Length == 6)
            {
                color += "FF"; // add alpha if missing
            }

            var hex = Convert.ToUInt32(color, 16);
            var r = ((hex & 0xff000000) >> 0x18) / 255f;
            var g = ((hex & 0xff0000) >> 0x10) / 255f;
            var b = ((hex & 0xff00) >> 8) / 255f;
            var a = ((hex & 0xff)) / 255f;

            return new Color(r, g, b, a);
        }
    }
}