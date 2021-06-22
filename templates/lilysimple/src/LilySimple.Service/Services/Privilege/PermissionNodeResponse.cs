using LilySimple.DataStructure.Tree;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LilySimple.Services.Privilege
{
    public class PermissionNodeResponse : TreeNode
    {
        [JsonPropertyName("id")]
        public override int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("pid")]
        public override int ParentId { get; set; }
    }
}
