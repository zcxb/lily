using LilySimple.Authorizations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilySimple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : BizControllerBase
    {
        [HttpGet]
        [Permission("action1")]
        public async Task<ActionResult> Action1()
        {
            throw new NotImplementedException();
        }
    }
}
