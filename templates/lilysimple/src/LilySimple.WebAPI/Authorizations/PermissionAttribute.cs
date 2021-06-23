using LilySimple.Authorizations;
using LilySimple.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Result = new JsonResult(new Flag().Fail("Permission denied!"));
            }
        }
    }
}
