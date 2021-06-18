using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Models
{
    public class RolePermission : ModelBase
    {
        public int RoleId { get; set; }

        public int PermissionId { get; set; }
    }
}
