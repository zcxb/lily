﻿using LilySimple.DataStructure.Tree;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilySimple.Services.Privilege
{
    public class PrivilegeService : ServiceBase
    {
        private readonly ILogger<PrivilegeService> _logger;

        public PrivilegeService(ILogger<PrivilegeService> logger)
        {
            _logger = logger;
        }

        public Task<bool> CheckPermission(int userId, string permissionName)
        {
            var roles = Db.UserRoles.Where(i => i.UserId == userId).Select(i => i.RoleId).ToList();
            if (roles.IsNullOrEmpty())
            {
                return Task.FromResult(false);
            }

            var permission = Db.Permissions.Where(i => i.Code == permissionName);
            var result = Db.RolePermissions.Where(i => roles.Contains(i.RoleId))
                .Join(permission, rp => rp.PermissionId, p => p.Id, (rp, p) => p)
                .Any();

            return Task.FromResult(result);
        }

        public Task<Listed<PermissionNodeResponse>> GetPermissionTree()
        {
            var permissions = Db.Permissions.Select(i => new PermissionNodeResponse
            {
                Id = i.Id,
                Name = i.Name,
                ParentId = i.ParentId,
            }).ToList().BuildTree();
            var response = new Listed<PermissionNodeResponse>();
            response.SetSuccess(permissions);

            return Task.FromResult(response);
        }

        public Task<Paginated<PermissionEntityResponse>> GetPermissionList()
        {
            var response = new Paginated<PermissionEntityResponse>();

            return Task.FromResult(response);
        }
    }
}
