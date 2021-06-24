using LilySimple.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilySimple.Areas.Rbac.Controllers
{
    [Area("Rbac")]
    [ApiExplorerSettings(GroupName = "Rbac")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class RbacAreaControllerBase : BizControllerBase
    {
    }
}
