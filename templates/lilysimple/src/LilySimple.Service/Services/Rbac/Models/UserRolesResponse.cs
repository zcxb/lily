using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Services.Rbac
{
    public class UserRolesResponse
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public int[] Roles { get; set; }
    }
}
