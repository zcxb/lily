using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LilySimple.Services.Rbac
{
    public class UserEntityResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("username")]
        public string UserName { get; set; }
    }
}
