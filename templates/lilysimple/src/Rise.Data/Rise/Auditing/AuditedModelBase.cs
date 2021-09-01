using Rise.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rise.Auditing
{
    public abstract class AuditedModelBase : ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}
