using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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

        public override void Register()
        {
            if (Registered) return;
            if (!Application.isPlaying && !ActiveInEditor) return;
            Gates.AddRange(GetComponents<ActiveGate>());
            foreach(var g in Gates)
                g.Register(this);
        }

        public override void DidRegister()
        {
            Check();
        }

        public override void Unregister()
        {
            foreach(var g in Gates)
                g.Unregister();            
        }

        public void Check() 
        {
            if (!Application.isPlaying && !ActiveInEditor) return;
            if (Gates.Count == 0) return;
            var active = Op == Operation.And;
            foreach(var g in Gates) 
            {
                if (!g.enabled) continue;
                var check = g.Check();
                if (check == null) continue;
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
                        yield return null;
                        var info = anim.GetCurrentAnimatorClipInfo(0);
                        if (info != null && info.Length > 0)
                            yield return new WaitForSeconds(info[0].clip.length);                   
                    }
                } else if (FadeExitDuration > 0f)
                {
                    yield return new WaitForTween(LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 0f,
                        FadeExitDuration));
                }
            }
            gameObject.SetActive(active);
        }
    }
}