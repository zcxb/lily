using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LilySimple.Models.Role
{
    public class RoleCreateRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("permissions")]
        public int[] Permissions { get; set; }
    }
}
