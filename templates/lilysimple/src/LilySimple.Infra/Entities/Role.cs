using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Entities
{
    public class Role : ModelBase
    {
        public string Name { get; set; }

        public bool IsReserved { get; set; }
    }
}
