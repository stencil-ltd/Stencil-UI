using UnityEngine;

namespace Util.Coroutines
{
    public class WaitForParticle : CustomYieldInstruction
    {
        public readonly ParticleSystem particle;

        public WaitForParticle(ParticleSystem particle)
        {
            this.particle = particle;
        }

        public override bool keepWaiting => particle != null && !particle.isStopped;
    }
}