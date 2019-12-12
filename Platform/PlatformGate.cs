using System.Linq;
using State.Active;
using UnityEngine;

namespace Stencil.Ui.Platform
{
    public class PlatformGate : ActiveGate
    {
        public RuntimePlatform[] platforms;

        public override void Register(ActiveManager manager)
        {
            base.Register(manager);
            RequestCheck();
        }

        public override bool? Check()
        {
            var platform = Application.platform;
            return platforms.Contains(platform);
        }
    }
}