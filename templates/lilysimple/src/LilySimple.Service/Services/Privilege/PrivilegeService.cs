using LilySimple.DataStructure.Tree;
using LilySimple.Models;
using LilySimple.Shared.Enums;
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

        public Task<Paginated<PermissionEntityResponse>> GetPermissionList(int page, int pageSize)
        {
            var response = new Paginated<PermissionEntityResponse>();
            var query = Db.Permissions;
            var entities = query.PageByNumber(page, pageSize).ToList();
            var count = query.Count();

            response.SetSuccess(entities.Select(i => new PermissionEntityResponse
            {
                Id = i.Id,
                Name = i.Name,
                Code = i.Code,
                ParentId = i.ParentId,
                Path = i.Path,
                Type = ((PermissionType)i.Type).GetDescription(),
            }), count);
            return Task.FromResult(response);
        }

        public Task<Wrapped<Id>> CreatePermission(string name, string code, string path, string type, int parentId)
        {
            var response = new Wrapped<Id>();

            var model = Permission.Create(name, code, path, parentId, type.ToEnumValue<PermissionType>());
            var entity = Db.Permissions.Add(model).Entity;

            if (Db.SaveChanges() > 0)
            {
                response.SetSuccess(new Id(entity.Id));
            }

            return Task.FromResult(response);
        }
    }
}
