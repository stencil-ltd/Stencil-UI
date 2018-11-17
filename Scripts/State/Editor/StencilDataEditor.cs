using UnityEditor;
using UnityEngine;

namespace State.Editor
{
    [CustomEditor(typeof(StencilData), true)]
    public class StencilDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            if (GUILayout.Button("Reload Ids"))
            {
                Debug.Log("Reloading all StencilData ids");
                StencilData.Reload();
            }
        }
    }
}