using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LilySimple.Models.User
{
    public class UserQueryRequest : Pager
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }
    }
}
