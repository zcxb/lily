using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Services.Privilege
{
    public class PermissionEntityResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Path { get; set; }

        public int ParentId { get; set; }

        public string Type { get; set; }
    }
}
