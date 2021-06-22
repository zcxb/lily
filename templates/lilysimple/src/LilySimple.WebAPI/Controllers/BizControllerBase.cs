using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LilySimple.Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LilySimple.Controllers
{
    [Authorize]
    public class BizControllerBase : ControllerBase
    {
        protected readonly IConfiguration Configuration;

        public BizControllerBase()
        {
            Configuration = IocManager.Instance.GetService<IConfiguration>();
        }
    }
}