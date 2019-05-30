using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Scripts.Util;
using UI;
using UnityEngine;
using Util;
using Util.Coroutines;

namespace State.Active
{
    public enum Operation
    {
        And, Or
    }

    public class ActiveManager : RegisterableBehaviour
    {
        public Operation Op = Operation.Or;

        public readonly List<ActiveGate> Gates = new List<ActiveGate>();
        public bool ActiveInEditor;

        [Header("Transitions")] 
        public float FadeExitDuration;
        [CanBeNull] public string ExitTrigger;
        
        [Header("Debug")]
        public bool Logging = false;

        private void Start()
        {
            // Get the enabled checkbox.
        }

        public void RemoveAll(bool andDestroy = false)
        {
            Gates.Clear();
            foreach (var activeGate in GetComponents<ActiveGate>())
            {
                Destroy(activeGate);
            }
            if (andDestroy) Destroy(this);
        }

        public void RemoveGate(ActiveGate gate)
        {
            Gates.Remove(gate);
            Destroy(gate);
        }

        public override void Register()
        {
            if (Registered) return;
            if (!Application.isPlaying && !ActiveInEditor) return;
            Gates.AddRange(GetComponents<ActiveGate>());
            foreach(var g in Gates) g.Register(this);
        }

        public override void DidRegister()
        {
            base.DidRegister();
            foreach(var g in Gates) g.DidRegister();
            Check();
        }

        public override void Unregister()
        {
            base.Unregister();
            foreach(var g in Gates) g.Unregister();
            Registered = false;
        }

        public override void WillUnregister()
        {
            foreach(var g in Gates) g.WillUnregister();    
            base.WillUnregister();
        }

        public void Check() 
        {
            if (!Application.isPlaying && !ActiveInEditor) return;
            if (Gates.Count == 0) return;
            if (this == null || !enabled) return;
            var active = Op == Operation.And;
            var hasActive = false;
            foreach(var g in Gates) 
            {
                if (g == null || !g.enabled) continue;
                var check = g.Check();
                if (check == null) continue;
                hasActive = true;
                switch(Op)
                {
                    case Operation.And:
                        active &= check.Value;
                        break;
                    case Operation.Or:
                        active |= check.Value;
                        break;
                }
            }
            if (hasActive)
                Objects.StartCoroutine(SetActiveInternal(active));
        }

        private IEnumerator SetActiveInternal(bool active)
        {
            if (active == gameObject.activeSelf) yield break;
            if (!active && !string.IsNullOrEmpty(ExitTrigger))
            {
                if (!string.IsNullOrEmpty(ExitTrigger))
                {
                    var anim = GetComponent<Animator>();
                    if (anim != null)
                    {
                        anim.SetTrigger(ExitTrigger);
                        yield return anim.Await(0.1f);
                    }
                } else if (FadeExitDuration > 0f)
                {
                    yield return new WaitForTween(LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 0f,
                        FadeExitDuration));
                }
            }
            MaybeLog($"SetActiveInternal set active {active}");
            gameObject.SetActive(active);
        }

        private void MaybeLog(String message)
        {
            if (Logging) 
                Debug.Log($"[{name}]ActiveManager - {message}");
        }
    }
}