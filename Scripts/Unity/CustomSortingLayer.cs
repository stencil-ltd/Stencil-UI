using Binding;
using UnityEngine;

namespace Unity
{
    [RequireComponent(typeof(Canvas))]
    public class CustomSortingLayer : MonoBehaviour
    {
//        private static readonly IDictionary<string, int> _ids = new Dictionary<string, int>();

        [Bind] private Canvas _canvas;

        public string sortingLayer;

        private void Awake()
        {
            this.Bind();
            _canvas.overrideSorting = true;
            _canvas.sortingLayerName = sortingLayer;
        }
    }
}