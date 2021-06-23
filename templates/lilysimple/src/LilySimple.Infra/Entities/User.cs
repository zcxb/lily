using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Entities
{
    public class User : ModelBase
    {
        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
        }
    }
}
