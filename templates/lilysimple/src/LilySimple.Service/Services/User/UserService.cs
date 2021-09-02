using LilySimple.EntityFrameworkCore;
using LilySimple.Services;
using LilySimple.Settings;
using LilySimple.Shared.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserModel = LilySimple.Entities.User;

namespace LilySimple.Services
{
    public partial class ErrorCode
    {
        public const int WrongPassword = 3001;

    }

    public class UserService : ServiceBase
    {
        private readonly RbacService _rbacService;
        private readonly JwtBearerSetting _jwtBearerSetting;

        public UserService(RbacService rbacService,
                               IOptions<JwtBearerSetting> jwtBearerSetting)
        {
            _rbacService = rbacService;
            _jwtBearerSetting = jwtBearerSetting?.Value ?? throw new ArgumentNullException(nameof(jwtBearerSetting));
        }

        public Task<R> Login(UserLoginRequest request)
        {
            IList<Claim> claims = new List<Claim>();

            var entity = Db.Users.Where(u => u.UserName == request.UserName).FirstOrDefault();
            if (entity == null)
            {
                return Task.FromResult(R.Error(ErrorCode.UserNotFound, nameof(ErrorCode.UserNotFound)));
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, entity.PasswordHash))
            {
                Logger.LogDebug("user {UserName}[{UserId}] login failed", request.UserName, entity.Id);
                return Task.FromResult(R.Error(ErrorCode.WrongPassword, nameof(ErrorCode.WrongPassword)));
            }

            claims.Add(new Claim("sub", entity.Id.ToString()));
            claims.Add(new Claim("username", entity.UserName));

            return Task.FromResult(R.Object(new UserLoginResponse
            {
                AccessToken = GenerateJwtToken(claims),
                ExpiresIn = _jwtBearerSetting.Expires,
            }));
        }

        private string GenerateJwtToken(IEnumerable<Claim> claims)
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

        public Task<R> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var entity = Db.Users.GetById(userId).FirstOrDefault();
            if (entity == null)
            {
                return Task.FromResult(R.Error(ErrorCode.UserNotFound, nameof(ErrorCode.UserNotFound)));
            }
            if (!BCrypt.Net.BCrypt.Verify(oldPassword, entity.PasswordHash))
            {
                return Task.FromResult(R.Error(ErrorCode.WrongPassword, nameof(ErrorCode.WrongPassword)));
            }
            try
            {
                entity.ChangePassword(BCrypt.Net.BCrypt.HashPassword(newPassword));
                Db.SaveChanges();
                return Task.FromResult(R.Ok());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public Task<R> GetUserProfile(int userId, bool isAdmin)
        {
            var userEntity = Db.Users.GetById(userId).FirstOrDefault();
            if (userEntity == null)
            {
                return Task.FromResult(R.Error(ErrorCode.UserNotFound, nameof(ErrorCode.UserNotFound)));
            }
            var permissions = _rbacService.GetTreePermissions(isAdmin ? 0 : userId);
            return Task.FromResult(R.Object(new UserProfileResponse
            {
                UserName = userEntity.UserName,
                Permissions = permissions
            }));
        }
    }
}
