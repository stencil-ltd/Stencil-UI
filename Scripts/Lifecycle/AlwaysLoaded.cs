using System.Linq;
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
    }
}