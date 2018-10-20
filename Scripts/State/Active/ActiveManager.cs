using System.Collections.Generic;
using UI;
using UnityEngine;

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
            gameObject.SetActive(active);
        }
    }
}