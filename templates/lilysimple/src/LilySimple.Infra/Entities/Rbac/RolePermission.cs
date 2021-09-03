using Rise.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Entities
{
    public class RolePermission : AuditedModelBase
    {
        public int RoleId { get; set; }

        public int PermissionId { get; set; }

        public static RolePermission Create(int roleId, int permissionId)
        {
            return new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId,
            };
        }
    }
}
