﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LilySimple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : BizControllerBase
    {
        public AlbumsController()
        {
        }
    }
}