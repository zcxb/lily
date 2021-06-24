using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LilySimple.Services.Rbac
{
    public class PermissionEntityResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("pid")]
        public int ParentId { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
