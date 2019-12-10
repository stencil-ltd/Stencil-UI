using System.Globalization;
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
            var selected = Selection.activeTransform;
            var idx = selected.GetSiblingIndex();
            var childRect = selected as RectTransform;
            Undo.RecordObject(selected.gameObject, "Create Parent Wrapper");
            var parentTransform = new GameObject($"{selected.name} Parent", selected.GetType()).transform;
            Undo.RegisterCreatedObjectUndo(parentTransform.gameObject, "Create wrapper object");
            Undo.SetTransformParent(parentTransform, selected.transform.parent, "Set Grandparent");
            parentTransform.SetSiblingIndex(idx);
            parentTransform.localPosition = selected.localPosition;
            parentTransform.localScale = selected.localScale;
            parentTransform.localRotation = selected.localRotation;
            if (childRect != null)
            {
                var parentRect = (RectTransform) parentTransform;
                parentRect.anchorMin = childRect.anchorMin;
                parentRect.anchorMax = childRect.anchorMax;
                parentRect.offsetMin = childRect.offsetMin;
                parentRect.offsetMax = childRect.offsetMax;
                parentRect.pivot = childRect.pivot;
            }

            Undo.SetTransformParent(selected.transform, parentTransform, "Reparent to wrapper");
            if (childRect != null)
            {
                childRect.anchorMin = Vector2.zero;
                childRect.anchorMax = Vector2.one;
                childRect.offsetMin = Vector2.zero;
                childRect.offsetMax = Vector2.zero;
                childRect.pivot = new Vector2(0.5f, 0.5f);
            }
            
            Selection.activeGameObject = parentTransform.gameObject;
            Undo.CollapseUndoOperations(group);
        }
    }
}