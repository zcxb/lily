using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Models
{
    public class Song : ModelBase
    {
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}
