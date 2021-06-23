using LilySimple.Services.Privilege;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilySimple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : BizControllerBase
    {
        private readonly PrivilegeService _privilegeService;

        public RolesController(PrivilegeService privilegeService)
        {
            _privilegeService = privilegeService;
        }


    }
}
