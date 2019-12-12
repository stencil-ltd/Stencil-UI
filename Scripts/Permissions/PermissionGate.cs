using System;
using State.Active;
using Stencil.Permissions;

namespace Stencil.Ui.Permissions
{
    public class PermissionGate : ActiveGate
    {
        public Permission[] permissions;
        
        private StencilPermissions _perms = new StencilPermissions();
        
        public override bool? Check()
        {
            foreach (var permission in permissions)
                if (!_perms.HasPermission(permission))
                    return false;
            return true;
        }

        public override void Register(ActiveManager manager)
        {
            base.Register(manager);
            RequestCheck();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus) RequestCheck();
        }
    }
}