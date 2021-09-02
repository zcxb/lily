using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilySimple.Settings
{
    public class JwtBearerSetting
    {
        public string Issuer { get; set; }

        public string SecurityKey { get; set; }

        public int Expires { get; set; }
    }
}
