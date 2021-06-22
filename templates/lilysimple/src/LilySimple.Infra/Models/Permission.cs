using LilySimple.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Models
{
    public class Permission : ModelBase
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Path { get; set; }

        public int ParentId { get; set; }

        public int Type { get; set; }

        public static Permission Create(string name, string code, string path, int parentId, PermissionType type)
        {
            return new Permission
            {
                Name = name,
                Code = code,
                Path = path,
                ParentId = parentId,
                Type = type.GetHashCode(),
            };
        }
    }
}
