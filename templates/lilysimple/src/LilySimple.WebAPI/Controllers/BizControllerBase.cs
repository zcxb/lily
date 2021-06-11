using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LilySimple.Controllers
{
    [Authorize]
    public class BizControllerBase : ControllerBase
    {
        protected int UserId
        {
            get
            {
                var userIdOrNull = User.Claims?.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdOrNull?.Value, out int result))
                {
                    return result;
                }
                return 0;
            }
        }
    }
}