using Rise.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Entities
{
    public class Role : AuditedModelBase
    {
        public string Name { get; set; }

        public bool IsReserved { get; set; }

        public static Role Create(string name, bool isReserved = false)
        {
            return new Role
            {
                Name = name,
                IsReserved = isReserved,
            };
        }

        public void Modify(string name)
        {
            Name = name;
        }
    }
}
