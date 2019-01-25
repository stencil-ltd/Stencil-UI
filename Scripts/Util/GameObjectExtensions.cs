using JetBrains.Annotations;
using UnityEngine;

namespace Util
{
    public static class GameObjectExtensions
    {
        public static bool IsPermanent(this GameObject obj)
            => obj.scene.buildIndex < 0;

        public static bool IsPermanent(this Component comp)
            => comp != null && comp.gameObject != null && comp.gameObject.IsPermanent();
    }
}