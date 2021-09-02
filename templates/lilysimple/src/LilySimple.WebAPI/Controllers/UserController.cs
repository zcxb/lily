using LilySimple.Extensions;
using LilySimple.Services;
using LilySimple.Settings;
using LilySimple.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LilySimple.Authorizations;

namespace LilySimple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BizControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] UserLoginRequest request)
        {
            return Ok(await _userService.Login(request));
        }

        /// <summary>
        /// 获取用户详细信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("profile")]
        public async Task<ActionResult> GetUserProfile()
        {
            var result = await _userService.GetUserProfile(User.GetUserId(), User.IsAdmin());
            return Ok(result);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = await _userService.ChangePassword(User.GetUserId(), request.OldPassword, request.NewPassword);
            return Ok(result);
        }
    }
}