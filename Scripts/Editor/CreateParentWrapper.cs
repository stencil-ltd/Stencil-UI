using UnityEditor;
using UnityEngine;
using Util;

namespace Stencil.UI.Editor
{
    public static class CreateParentWrapper
    {
        [MenuItem("GameObject/Create Parent Wrapper", false, 0)]
        public static void Wrap()
        {
            Undo.SetCurrentGroupName("Create Parent Wrapper");
            var group = Undo.GetCurrentGroup();
            var selected = Selection.activeGameObject;
            Undo.RecordObject(selected.gameObject, "Create Parent Wrapper");
            var parentTransform = Object.Instantiate(selected.transform, selected.transform.parent, true);
            Undo.RegisterCreatedObjectUndo(parentTransform.gameObject, "Create wrapper object");
            Undo.SetTransformParent(selected.transform, parentTransform, "Reparent to wrapper");
            parentTransform.gameObject.name = $"{selected.name} Parent";
            var rectTransform = selected.transform as RectTransform;
            if (rectTransform != null)
            {
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
            }
            Selection.activeGameObject = parentTransform.gameObject;
            Undo.CollapseUndoOperations(group);
        }
    }
}