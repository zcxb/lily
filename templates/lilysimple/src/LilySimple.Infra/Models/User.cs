using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Models
{
    public class User : ModelBase
    {
        public string UserName { get; set; }

        public string PasswordHash { get; set; }
    }
}
