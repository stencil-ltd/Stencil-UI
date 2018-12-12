#if !EXCLUDE_ODIN
using Sirenix.OdinInspector.Editor;
#endif

using UnityEditor;
using UnityEngine;

namespace State.Editor
{
    [CustomEditor(typeof(StencilData), true)]
    #if !EXCLUDE_ODIN
    public class StencilDataEditor : OdinEditor
    #else
    public class StencilDataEditor : UnityEditor.Editor
    #endif
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            if (GUILayout.Button("Reload Ids"))
            {
                Debug.Log("Reloading all StencilData ids");
                StencilData.Reload();
            }
        }
    }
}