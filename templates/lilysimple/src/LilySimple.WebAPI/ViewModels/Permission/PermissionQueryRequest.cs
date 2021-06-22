using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilySimple.ViewModels.Permission
{
    public class PermissionQueryRequest : Pager
    {
        [ModelBinder(Name = "name")]
        public string Name { get; set; }

        [ModelBinder(Name = "code")]
        public string Code { get; set; }

        [ModelBinder(Name = "path")]
        public string Path { get; set; }

        [ModelBinder(Name = "type")]
        public string Type { get; set; }
    }
}
