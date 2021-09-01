﻿using LilySimple.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LilySimple.Services
{
    public class PermissionModifyRequest
    {
        [Required]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

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
