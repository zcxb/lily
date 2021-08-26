using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Entities
{
    public class UserRole : ModelBase
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public static UserRole Create(int userId, int roleId)
        {
            return new UserRole
            {
                UserId = userId,
                RoleId = roleId,
            };
        }
    }
}
