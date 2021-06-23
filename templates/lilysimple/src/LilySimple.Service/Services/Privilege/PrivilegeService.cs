using LilySimple.DataStructure.Tree;
using LilySimple.Entities;
using LilySimple.EntityFrameworkCore;
using LilySimple.Shared.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoleModel = LilySimple.Entities.Role;

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
                Code = i.Code,
                Path = i.Path,
                ParentId = i.ParentId,
                Type = ((PermissionType)i.Type).GetDescription(),
            }).ToList().BuildTree();
            var response = new Listed<PermissionNodeResponse>();
            response.Succeed(permissions);

            return Task.FromResult(response);
        }

        public Task<Paginated<PermissionEntityResponse>> GetPermissionList(int page, int pageSize)
        {
            var response = new Paginated<PermissionEntityResponse>();

            var query = Db.Permissions;
            var entities = query.PageByNumber(page, pageSize).ToList();
            var count = query.Count();

            response.Succeed(entities.Select(i => new PermissionEntityResponse
            {
                Id = i.Id,
                Name = i.Name,
                Code = i.Code,
                Path = i.Path,
                ParentId = i.ParentId,
                Type = ((PermissionType)i.Type).GetDescription(),
            }), count);

            return Task.FromResult(response);
        }

        public Task<Wrapped<PermissionEntityResponse>> GetPermissionById(int id)
        {
            var response = new Wrapped<PermissionEntityResponse>();

            var entity = Db.Permissions.GetById(id);
            if (entity == null)
            {
                response.Fail("Permission not exist");
            }
            else
            {
                response.Succeed(new PermissionEntityResponse
                {
                    Id = id,
                    Name = entity.Name,
                    Code = entity.Code,
                    Path = entity.Path,
                    ParentId = entity.ParentId,
                    Type = ((PermissionType)entity.Type).GetDescription(),
                });
            }

            return Task.FromResult(response);
        }

        public Task<Wrapped<Id>> CreatePermission(string name, string code, string path, int parentId, string type)
        {
            var response = new Wrapped<Id>();

            var model = Permission.Create(name, code, path, parentId, type.ToEnumValue<PermissionType>());
            var entity = Db.Permissions.Add(model).Entity;
            if (Db.SaveChanges() > 0)
            {
                response.Succeed(new Id(entity.Id));
            }

            return Task.FromResult(response);
        }

        public Task<Flag> ModifyPermission(int id, string name, string code, string path, int parentId, string type)
        {
            var response = new Flag();

            var entity = Db.Permissions.GetById(id);
            if (entity == null)
            {
                response.Fail("Permission not exist");
            }
            else if (Db.Permissions.GetById(parentId) == null)
            {
                response.Fail("Parent permission not exist");
            }
            else
            {
                entity.Modify(name, code, path, parentId, type.ToEnumValue<PermissionType>());
                if (Db.SaveChanges() > 0)
                {
                    response.Succeed();
                }
            }

            return Task.FromResult(response);
        }

        public Task<Flag> DeletePermission(int id)
        {
            var response = new Flag();

            var entity = Db.Permissions.GetById(id);
            if (entity == null)
            {
                response.Fail("Permission not exist");
            }
            else
            {
                Db.Permissions.Remove(entity);
                if (Db.SaveChanges() > 0)
                {
                    response.Succeed();
                }
            }

            return Task.FromResult(response);
        }

        public Task<Wrapped<Id>> CreateRole(string name, int[] permissions)
        {
            var response = new Wrapped<Id>();

            using var trans = Db.Database.BeginTransaction();
            try
            {
                var roleModel = RoleModel.Create(name);
                var roleEntity = Db.Roles.Add(roleModel).Entity;
                Db.SaveChanges();
                var rolePermissionModels = permissions.Select(i => RolePermission.Create(roleEntity.Id, i)).ToList();
                Db.RolePermissions.AddRange(rolePermissionModels);
                Db.SaveChanges();

                trans.Commit();
                response.Succeed(new Id(roleEntity.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                trans.Rollback();
                response.Fail("Failed to create new role and permissions");
            }

            return Task.FromResult(response);
        }

        public Task<Flag> ModifyRole(int id, string name, int[] permissions)
        {
            var response = new Flag();

            using var trans = Db.Database.BeginTransaction();
            try
            {
                var roleEntity = Db.Roles.GetById(id);
                if (roleEntity == null)
                {
                    response.Fail("Role not exist");
                }
                else
                {
                    roleEntity.Modify(name);
                    var oldPermissions = Db.RolePermissions.Where(i => i.RoleId == id).Select(i => i.PermissionId).ToList();
                    Db.SaveChanges();
                    Db.RolePermissions.RemoveRange(oldPermissions.Except(permissions).ToArray());
                    Db.SaveChanges();
                    var newPermissions = permissions.Except(oldPermissions).Select(i => RolePermission.Create(id, i));
                    Db.RolePermissions.AddRange(newPermissions);
                    Db.SaveChanges();
                }
                trans.Commit();
                response.Succeed();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                trans.Rollback();
                response.Fail("Failed to modify role and permissions");
            }

            return Task.FromResult(response);
        }

        public Task<Flag> DeleteRole(int id)
        {
            var response = new Flag();

            var entity = Db.Roles.GetById(id);
            if (entity == null)
            {
                response.Fail("Role not exist");
            }

            return Task.FromResult(response);
        }
    }
}
