using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Lifecycle
{
    public class AlwaysScriptableObject : ScriptableObject
    {
#if UNITY_EDITOR
        [MenuItem("Stencil/Always Loaded")]
#endif
        public static void Refresh()
        {
            var obj = GameObject.Find("Always Loaded");
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