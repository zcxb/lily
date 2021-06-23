using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Entities
{
    public class Role : ModelBase
    {
        public string Name { get; set; }

        public bool IsReserved { get; set; }

        public static Role Create(string name)
        {
            return new Role
            {
                Name = name,
                IsReserved = false,
            };
        }

        public void Modify(string name)
        {
            Name = name;
        }
    }
}
