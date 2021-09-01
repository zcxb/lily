using LilySimple.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LilySimple.Services
{
    public class PermissionCreateRequest
    {
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 权限code，由后端接口指定
        /// </summary>
        [Required]
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("pid")]
        public int ParentId { get; set; }

        /// <summary>
        /// 权限类型：resource/action
        /// </summary>
        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("sort")]
        public int Sort { get; set; }
    }
}
