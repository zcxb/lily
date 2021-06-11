using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LilySimple.ViewModels.User
{
    public class ChangePasswordRequest
    {
        [JsonPropertyName("old_password")]
        public string OldPassword { get; set; }

        [JsonPropertyName("new_password")]
        public string NewPassword { get; set; }
    }
}
