using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Entities
{
    public class ModelBase : ISoftDelete
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime InsertedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
