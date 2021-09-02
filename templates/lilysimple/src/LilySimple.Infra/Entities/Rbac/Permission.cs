using LilySimple.Shared.Enums;
using Rise.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Entities
{
    public class Permission : AuditedModelBase
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Path { get; set; }

        public int ParentId { get; set; }

        public int Type { get; set; }

        public int Sort { get; set; }

        public static Permission Create(string name, string code, string path, int parentId, PermissionType type, int sort)
        {
            return new Permission
            {
                Name = name,
                Code = code,
                Path = path,
                ParentId = parentId,
                Type = type.GetHashCode(),
                Sort = sort,
            };
        }

        public void Modify(string name, string code, string path, int parentId, PermissionType type, int sort)
        {
            Name = name;
            Code = code;
            Path = path;
            ParentId = parentId;
            Type = type.GetHashCode();
            Sort = sort;
        }
    }
}
