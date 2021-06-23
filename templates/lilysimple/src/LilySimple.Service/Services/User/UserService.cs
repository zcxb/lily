using LilySimple.EntityFrameworkCore;
using LilySimple.Shared.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserModel = LilySimple.Models.User;

namespace LilySimple.Services.User
{
    public class UserService : ServiceBase
    {
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public void InitializeAdminUser()
        {
            var adminUserName = Configuration["AdminInit:UserName"] ?? "admin";
            var adminPassword = Configuration["AdminInit:Password"] ?? "123456";

            try
            {
                if (Db.Users.Any(i => i.UserName.Equals(adminUserName)))
                {
                    _logger.LogInformation("admin account exists, quit init process");
                    return;
                }

                var model = new UserModel
                {
                    UserName = adminUserName,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword)
                };
                var entity = Db.Users.Add(model).Entity;
                if (Db.SaveChanges() > 0)
                {
                    _logger.LogInformation("admin account has been created.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public Task<(UserLoginStatus, Claim[])> ValidateLoginUser(string userName, string password)
        {
            IList<Claim> claims = new List<Claim>();

            var entity = Db.Users.Where(i => i.UserName == userName).FirstOrDefault();
            if (entity == null)
            {
                return Task.FromResult((UserLoginStatus.AccountNotFound, claims.ToArray()));
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(password, entity.PasswordHash);
            if (isValid)
            {
                claims.Add(new Claim("sub", entity.Id.ToString()));
                claims.Add(new Claim("username", entity.UserName));
                return Task.FromResult((UserLoginStatus.Success, claims.ToArray()));
            }
            else
            {
                return Task.FromResult((UserLoginStatus.WrongPassword, claims.ToArray()));
            }
        }

        public Task<Flag> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var result = new Flag();

            var entity = Db.Users.GetById(userId);
            if (entity == null)
            {
                result.Fail("用户不存在");
            }
            else if (!BCrypt.Net.BCrypt.Verify(oldPassword, entity.PasswordHash))
            {
                result.Fail("旧密码错误");
            }
            else
            {
                entity.ChangePassword(BCrypt.Net.BCrypt.HashPassword(newPassword));
                if (Db.SaveChanges() > 0)
                {
                    result.Succeed();
                }
            }

            return Task.FromResult(result);
        }

        //public Task<Wrapped<Object>> GetUserProfile()
        //{

        //}
    }
}
