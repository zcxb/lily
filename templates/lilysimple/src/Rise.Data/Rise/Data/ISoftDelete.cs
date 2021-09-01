using System;
using System.Collections.Generic;
using System.Text;

namespace Rise.Data
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
