using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Models
{
    public class Permission : ModelBase
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string Code { get; set; }

        public int Type { get; set; }
    }
}
