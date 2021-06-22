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
        [Permission("song-list")]
        public async Task<ActionResult> GetSongs()
        {
            throw new NotImplementedException();
        }
    }
}
