using System;
using System.Collections;
using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace State.Dynamic.UI
{
    public enum StateTextType
    {
        All, Prefix, Suffix
    }

    [RequireComponent(typeof(Text))]
    public class DynamicStateText : MonoBehaviour
    {
        public StateTextType StateTextType;
        public DynamicStateMachine StateMachine;

        [Bind] private Text _text;

        private void Awake()
        {
            this.Bind();
            StateMachine.OnChange += OnState;
        }

        private void OnDestroy()
        {
            StateMachine.OnChange -= OnState;   
        }

        private void OnState(object sender, DynamicStateChange e)
        {
            var text = _text.text;
            if (e.Old != null)
                text = StripOld(text, e.Old);
            text = AppendNew(text, e.New);
            _text.text = text;
        }

        private string AppendNew(string text, DynamicState @new)
        {
            var newName = @new.Name;
            switch (StateTextType)
            {
                case StateTextType.All:
                    return newName;
                case StateTextType.Prefix:
                    return $"{newName} {text}";
                case StateTextType.Suffix:
                    return $"{text} {newName}";
                default:
                    throw new Exception("Wat");
            }
        }

        private string StripOld(string text, DynamicState old)
        {
            var oldName = old.Name;
            switch (StateTextType)
            {
                case StateTextType.Prefix:
                    text = text.Replace($"{oldName} ", "");
                    break;
                case StateTextType.Suffix:
                    text = text.Replace($" {oldName}", "");
                    break;
            }
            return text;
        }

        // Use this for initialization
        void Start()
        {
            OnState(null, new DynamicStateChange(StateMachine.State, StateMachine.State));
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}