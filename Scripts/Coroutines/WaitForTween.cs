using UnityEngine;

namespace Util.Coroutines
{
    public class WaitForTween : CustomYieldInstruction
    {
        public readonly LTDescr tween;
        private bool complete = false;
        
        public WaitForTween(LTDescr tween)
        {
            this.tween = tween;
            tween.setOnComplete(() => complete = true);
        }

        public override bool keepWaiting => !complete;
    }
}