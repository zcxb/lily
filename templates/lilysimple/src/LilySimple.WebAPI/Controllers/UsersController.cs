﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LilySimple.Enums;
using LilySimple.Services;
using LilySimple.Settings;
using LilySimple.ViewModels;
using LilySimple.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LilySimple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BizControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtBearerSetting _jwtBearerSetting;

        public UsersController(UserService userService,
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
                response.SetError("wrong account or password");
            }
            else
            {
                var accessToken = GenerateJwtToken(claims);
                response.SetSuccess();
                response.Data = new UserLoginResponse
                {
                    AccessToken = accessToken,
                    ExpiresIn = _jwtBearerSetting.Expires,
                };
            }

            return Ok(response);
        }
    }
}