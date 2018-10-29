using System.Linq;
using UnityEngine;

namespace Util
{
    public static class Geo
    {
        public static void CastIntoUi(this Transform transform)
        {
            var mask = LayerMask.GetMask("UI");
            var pos = transform.position;
            var cam = Camera.main.transform.position;
            var ray = new Ray(pos, cam - pos);
            RaycastHit hit;
            var success = Physics.Raycast(ray, out hit, float.MaxValue, mask);
            if (success)
                transform.position = hit.point;
            else Debug.LogWarning($"Failed to cast lob into UI layer.");
        }
        
        public static Rect ToScreenSpace(this RectTransform transform)
        {
            var corners = new Vector3[4];
            transform.GetWorldCorners(corners);
            var width = (corners[3] - corners[0]).magnitude;
            var height = (corners[1] - corners[0]).magnitude;
            var rect = new Rect(corners[0].x, corners[0].y, width, height);
            rect.min = Camera.main.WorldToScreenPoint(rect.min);
            rect.max = Camera.main.WorldToScreenPoint(rect.max);
            return rect;
        }
        
        public static Vector3 Inverted(this Vector3 vec)
        {
            return new Vector3(1 / vec.x, 1 / vec.y, 1 / vec.z);
        }
        
        public static Canvas GetTopCanvas(this Component obj)
        {
            return obj.GetComponentsInParent<Canvas>().Last();
        }
        
        public static Vector3 GuessGlobalUiScale(this Transform transform)
        {
            return Vector3.Scale(transform.lossyScale, transform.GetTopCanvas().transform.localScale.Inverted());
        }
        
        public static Vector2 LocalToScreen(this RectTransform transform, Vector2 position)
        {
            var world = transform.TransformPoint(position);
            return RectTransformUtility.WorldToScreenPoint(Camera.main, world);
        }
        
        public static Rect Expand(this Rect rect, float by)
        {
            rect.xMin -= by;
            rect.xMax += by;
            rect.yMin -= by;
            rect.yMax += by;
            return rect;
        }
        
        public static RectTransform GetRectTransform(this GameObject obj)
        {
            return obj.transform as RectTransform;
        }
        public static RectTransform GetRectTransform(this MonoBehaviour obj)
        {
            return obj.transform as RectTransform;
        } 
    }
}