using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LilySimple.Services
{
    public class RolePermissionsRespose
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_reserved")]
        public bool IsReserved { get; set; }

        [JsonPropertyName("permissions")]
        public int[] Permissions { get; set; }
    }
}
