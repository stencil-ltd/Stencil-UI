using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Lifecycle
{
    public class AlwaysLoaded : MonoBehaviour
    {
        public Object[] Load;
        
        public static T1[] LoadItems<T1>() where T1 : ScriptableObject
        {
            return Resources.FindObjectsOfTypeAll<T1>().ToArray();
        }
        
#if UNITY_EDITOR
        [MenuItem("Stencil/Always Loaded")]
#endif
        public static void Refresh()
        {
            var obj = FindObjectOfType<AlwaysLoaded>()?.gameObject;
            if (obj == null)
            {
                obj = new GameObject("Always Loaded");
                obj.AddComponent<AlwaysLoaded>();
            }

            var always = Resources.FindObjectsOfTypeAll<AlwaysScriptableObject>()
                .Select(it => (Object) it)
                .ToArray();
            obj.GetComponent<AlwaysLoaded>().Load = always;
        }
    }
}