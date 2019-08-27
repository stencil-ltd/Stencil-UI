using System;
using System.Collections;
using System.Collections.Generic;
using Binding;
using JetBrains.Annotations;
using Scripts.Maths;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Scripts.Texts
{
    [RequireComponent(typeof(Text))]
    public class TypewriterText : MonoBehaviour
    {
        public float speed = 50f;
        
        [Bind] private Text _label;
        
        private string _text = "";
        private float _length = 0;
        private List<int> _boundaries = new List<int>();

        private void Awake()
        {
            this.Bind();
        }

        private void Update()
        {
            _length = (_length + Time.deltaTime * speed).AtMost(_text.Length);
            Refresh();
        }

        public async UniTask Await()
        {
            Debug.Log($"TypewriterText: Await {_text}");
            await UniTask.WaitUntil(IsFinished);
            Debug.Log($"TypewriterText: Finished {_text}");
        }

        public UniTask SetText(string text)
        {
            Debug.Log($"Typewriter: set text to '{text}'");
            _text = text ?? "";
            _length = 0;
            _boundaries = text.GetIndicesOf(" ", false);
            Debug.Log($"Boundaries: {_boundaries}");
            return Await();
        }
        
        public bool Skip()
        {
            if (!gameObject.activeInHierarchy) return false;
            if (string.IsNullOrEmpty(this._text)) return false;
            if (IsFinished()) return false;
            _length = _text.Length;
            _label.text = _text;
            return true;
        }

        private bool IsFinished()
        {
            return _label.text == _text;
        }

        private void Refresh()
        {
            _label.text = _text.Substring(0, (int) _length);
        }
    }
}