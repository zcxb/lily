using LilySimple.Extensions;
using LilySimple.Services.Privilege;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilySimple.Authorizations
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        private readonly ILogger<PermissionAuthorizationHandler> _logger;
        private readonly PrivilegeService _privilegeService;

        public PermissionAuthorizationHandler(ILogger<PermissionAuthorizationHandler> logger,
                                              PrivilegeService privilegeService)
        {
            _logger = logger;
            _privilegeService = privilegeService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            var user = context.User;
            if (user.IsAdmin())
            {
                context.Succeed(requirement);
                return;
            }

            var userId = user.GetUserId();
            var result = await _privilegeService.CheckPermission(userId, requirement.Name);
            if (result)
            {
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogWarning("user [{userId}] is not permitted for [{permissionName}].",
                    userId, requirement.Name);
                context.Fail();
            }
            return;
        }
    }
}
