using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LilySimple.QueryModels.Rbac
{
    public class UserQueryRequest : Pager
    {
        [ModelBinder(Name = "username")]
        public string UserName { get; set; }
    }
}
