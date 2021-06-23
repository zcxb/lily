using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LilySimple.Models.User
{
    public class UserLoginRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>admin</example>
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>123456</example>
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
