using Rise.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Entities
{
    public class User : AuditedModelBase
    {
        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public static User Create(string userName, string passwordHash)
        {
            return new User
            {
                UserName = userName,
                PasswordHash = passwordHash,
            };
        }

        public void Modify()
        {

        }

        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
        }
    }
}
