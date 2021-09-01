using LilySimple.EntityFrameworkCore;
using LilySimple.Services;
using LilySimple.Shared.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserModel = LilySimple.Entities.User;

namespace LilySimple.Services
{
    public class UserService : ServiceBase
    {
        private readonly RbacService _rbacService;

        public UserService(RbacService rbacService) 
        {
            _rbacService = rbacService;
        }

        public Task<(UserLoginStatus, Claim[])> ValidateLoginUser(string userName, string password)
        {
            IList<Claim> claims = new List<Claim>();

            var entity = Db.Users.Where(u => u.UserName == userName).FirstOrDefault();
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
                Logger.LogDebug("user {UserName}[{UserId}] login failed", userName, entity.Id);
                return Task.FromResult((UserLoginStatus.WrongPassword, claims.ToArray()));
            }
        }

        public Task<R> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var entity = Db.Users.GetById(userId).FirstOrDefault();
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

        public Task<R> GetUserProfile(int userId, bool isAdmin)
        {
            var userEntity = Db.Users.GetById(userId);
            if (userEntity == null)
            {
                result.Fail("User not exist");
            }
            else
            {
                var permissions = _rbacService.GetTreePermissions(isAdmin ? 0 : userId);

                result.Succeed(new UserProfileResponse
                {
                    UserName = userEntity.UserName,
                    Permissions = permissions
                });
            }

            return Task.FromResult(result);
        }
    }
}
