using LilySimple.Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LilySimple.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userIdOrNull = user.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdOrNull?.Value, out int result))
            {
                return result;
            }
            return 0;
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            var userName = user.Claims?.FirstOrDefault(c => c.Type == "username");
            if (userName != null && userName.Value.IsNotNullOrWhiteSpace())
            {
                return userName.Value;
            }
            return null;
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            var configuration = IocManager.Instance.GetService<IConfiguration>();
            return user.GetUserName() == configuration["AdminInit:UserName"];
        }
    }
}
