using LilySimple.Extensions;
using LilySimple.Services.Rbac;
using LilySimple.Settings;
using LilySimple.Shared.Enums;
using LilySimple.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LilySimple.Services;
using LilySimple.Authorizations;
using LilySimple.Services.User;

namespace LilySimple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BizControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtBearerSetting _jwtBearerSetting;

        public UserController(UserService userService,
                               IOptions<JwtBearerSetting> jwtBearerSetting)
        {
            _userService = userService;
            _jwtBearerSetting = jwtBearerSetting?.Value ?? throw new ArgumentNullException(nameof(jwtBearerSetting));
        }

        private string GenerateJwtToken(Claim[] claims)
        {
            var token = new JwtSecurityToken(
                    claims: claims,
                    issuer: _jwtBearerSetting.Issuer,
                    expires: DateTime.Now.AddSeconds(_jwtBearerSetting.Expires),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtBearerSetting.SecurityKey)),
                        SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(token);
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
            var response = new Wrapped<UserLoginResponse>();

            var (status, claims) = await _userService.ValidateLoginUser(request.UserName, request.Password);
            if (status == UserLoginStatus.AccountNotFound
                || status == UserLoginStatus.WrongPassword)
            {
                response.Fail("wrong account or password");
            }
            else
            {
                var accessToken = GenerateJwtToken(claims);
                response.Succeed(new UserLoginResponse
                {
                    AccessToken = accessToken,
                    ExpiresIn = _jwtBearerSetting.Expires,
                });
            }

            return Ok(response);
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