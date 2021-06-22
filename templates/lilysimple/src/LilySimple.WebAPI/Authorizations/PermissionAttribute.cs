using LilySimple.Authorizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilySimple.Authorizations
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public PermissionAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var service = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var result = await service.AuthorizeAsync(context.HttpContext.User, null, new PermissionAuthorizationRequirement(Name));
            if (!result.Succeeded)
            {
                throw new BizException("You are not allowed to access the resource or action!");
            }
        }
    }
}
