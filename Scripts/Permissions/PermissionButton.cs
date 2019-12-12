using System;
using Stencil.Permissions;
using UnityEngine;
using UnityEngine.UI;

namespace Stencil.Ui.Permissions
{
    [RequireComponent(typeof(Button))]
    public class PermissionButton : MonoBehaviour
    {
        public Permission permission;
        private StencilPermissions _perms = new StencilPermissions();
        
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => _perms.RequestPermission(permission));
        }
    }
}