using JetBrains.Annotations;
using UnityEngine;

namespace Util.Coroutines
{
    public class WaitForDestroy : CustomYieldInstruction
    {
        [CanBeNull] 
        private readonly GameObject gameObject;

        public WaitForDestroy([CanBeNull] GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public WaitForDestroy([CanBeNull] MonoBehaviour behaviour) : this(behaviour == null ? null : behaviour.gameObject) {}

        public override bool keepWaiting => gameObject != null;
    }
}