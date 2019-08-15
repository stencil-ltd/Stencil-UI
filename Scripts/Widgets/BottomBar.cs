using System;
using Scripts.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Widgets
{
    public class BottomBar : MonoBehaviour
    {
        private static readonly int Selected = Animator.StringToHash("Selected");
        
        public Button[] buttons { get; private set; }

        public int defaultSelection = -1;
        public IntUnityEvent onSelect;

        private int _selectedIndex = -1;

        public void Select(int index, bool animated = true)
        {
            if (_selectedIndex == index) return;
            _selectedIndex = index;
            var button = buttons[index];
            foreach (var button1 in buttons)
            {
                var selected = button == button1;
                var anim = button1.GetComponent<Animator>();
                anim.SetBool(Selected, selected);
//                if (selected && !animated) anim.Play(0, 0, 1f);
            }
            onSelect?.Invoke(index);
        }

        private void Awake()
        {
            buttons = GetComponentsInChildren<Button>(true);
            for (var i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                var x = i;
                button.transition = Selectable.Transition.None;
                button.onClick.AddListener(() => Select(x));
            }
        }

        private void Start()
        {
            if (defaultSelection >= 0)
                Select(defaultSelection, false);
        }
    }
}