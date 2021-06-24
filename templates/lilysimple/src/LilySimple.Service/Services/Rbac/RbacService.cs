using LilySimple.DataStructure.Tree;
using LilySimple.Entities;
using LilySimple.EntityFrameworkCore;
using LilySimple.Shared.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserModel = LilySimple.Entities.User;

namespace LilySimple.Services.Rbac
{
    public partial class RbacService : ServiceBase
    {
        private readonly ILogger<RbacService> _logger;

        public RbacService(ILogger<RbacService> logger)
        {
            _logger = logger;
        }

        public Task<Listed<PermissionNodeResponse>> GetFullTreePermissions()
        {
            var response = new Listed<PermissionNodeResponse>();
            response.Succeed(GetTreePermissions());
            return Task.FromResult(response);
        }

        public Task<Paginated<PermissionEntityResponse>> GetPaginatedPermissions(int page, int pageSize)
        {
            var response = new Paginated<PermissionEntityResponse>();

            var query = Db.Permissions;
            var entities = query.PageByNumber(page, pageSize)
                .Select(i => new PermissionEntityResponse
                {
                    Id = i.Id,
                    Name = i.Name,
                    Code = i.Code,
                    Path = i.Path,
                    ParentId = i.ParentId,
                    Type = ((PermissionType)i.Type).GetDescription(),
                    Sort = i.Sort,
                }).ToList();
            var count = query.Count();

            response.Succeed(entities, count);

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
                    Id = entity.Id,
                    Name = entity.Name,
                    Code = entity.Code,
                    Path = entity.Path,
                    ParentId = entity.ParentId,
                    Type = ((PermissionType)entity.Type).GetDescription(),
                    Sort = entity.Sort,
                });
            }

            return Task.FromResult(response);
        }

        public Task<Wrapped<Id>> CreatePermission(string name, string code, string path, int parentId, string type, int sort)
        {
            var response = new Wrapped<Id>();
            if (Db.Permissions.Any(p => p.Name == name || p.Code == code))
            {
                response.Fail("Duplicated name or code");
            }
            else if (parentId > 0 && Db.Permissions.GetById(parentId) == null)
            {
                response.Fail("Parent permission not exist");
            }
            else
            {
                var model = Permission.Create(name, code, path, parentId, type.ToEnumValue<PermissionType>(), sort);
                var entity = Db.Permissions.Add(model).Entity;
                if (Db.SaveChanges() > 0)
                {
                    response.Succeed(new Id(entity.Id));
                }
            }

            return Task.FromResult(response);
        }

        public Task<Flag> ModifyPermission(int id, string name, string code, string path, int parentId, string type, int sort)
        {
            var response = new Flag();

            var entity = Db.Permissions.GetById(id);
            if (entity == null)
            {
                response.Fail("Permission not exist");
            }
            else if (Db.Permissions.Where(p => p.Id != id).Any(p => p.Name == name || p.Code == code))
            {
                response.Fail("Duplicated name or code");
            }
            else if (parentId > 0 && Db.Permissions.GetById(parentId) == null)
            {
                response.Fail("Parent permission not exist");
            }
            else
            {
                entity.Modify(name, code, path, parentId, type.ToEnumValue<PermissionType>(), sort);
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
            else if (Db.Permissions.Any(p => p.ParentId == id))
            {
                response.Fail("You need to delete the child permissions first");
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

        public Task<Paginated<RoleEntityResponse>> GetPaginatedRoles(int page, int pageSize)
        {
            var response = new Paginated<RoleEntityResponse>();

            var query = Db.Roles;
            var entities = query.PageByNumber(page, pageSize)
                .Select(i => new RoleEntityResponse
                {
                    Id = i.Id,
                    Name = i.Name,
                    IsReserved = i.IsReserved,
                }).ToList();
            var count = query.Count();

            response.Succeed(entities, count);

            return Task.FromResult(response);
        }

        public Task<Wrapped<RolePermissionsRespose>> GetRolePermissionsByRoleId(int roleId)
        {
            var response = new Wrapped<RolePermissionsRespose>();

            var roleEntity = Db.Roles.GetById(roleId);
            if (roleEntity == null)
            {
                response.Fail("Role not exist");
            }
            else
            {
                var rolePermissions = Db.RolePermissions.Where(rp => rp.RoleId == roleId)
                    .Select(rp => rp.PermissionId).ToList();
                response.Succeed(new RolePermissionsRespose
                {
                    Id = roleEntity.Id,
                    Name = roleEntity.Name,
                    Permissions = rolePermissions.ToArray(),
                });
            }

            return Task.FromResult(response);
        }

        public Task<Wrapped<Id>> CreateRole(string name, int[] permissions)
        {
            var response = new Wrapped<Id>();

            using var trans = Db.Database.BeginTransaction();
            try
            {
                var roleModel = Role.Create(name);
                var roleEntity = Db.Roles.Add(roleModel).Entity;
                Db.SaveChanges();
                var rolePermissionModels = permissions
                    .Select(permissionsId => RolePermission.Create(roleEntity.Id, permissionsId)).ToList();
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

        public Task<Flag> ModifyRole(int roleId, string name, int[] permissions)
        {
            var response = new Flag();

            using var trans = Db.Database.BeginTransaction();
            try
            {
                var roleEntity = Db.Roles.GetById(roleId);
                if (roleEntity == null)
                {
                    response.Fail("Role not exist");
                }
                else
                {
                    roleEntity.Modify(name);
                    Db.SaveChanges();
                    var oldPermissions = Db.RolePermissions.Where(rp => rp.RoleId == roleId)
                        .Select(rp => rp.PermissionId).ToList();
                    Db.RolePermissions.RemoveRange(oldPermissions.Except(permissions).ToArray());
                    Db.SaveChanges();
                    var newPermissions = permissions.Except(oldPermissions)
                        .Select(permissionId => RolePermission.Create(roleId, permissionId));
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

        public Task<Flag> DeleteRole(int roleId)
        {
            var response = new Flag();

            var entity = Db.Roles.GetById(roleId);
            if (entity == null)
            {
                response.Fail("Role not exist");
            }

            return Task.FromResult(response);
        }

        public Task<bool> CheckUserPermission(int userId, string permissionName)
        {
            var roles = Db.UserRoles.Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId).ToList();
            if (roles.IsNullOrEmpty())
            {
                return Task.FromResult(false);
            }

            var permission = Db.Permissions.Where(p => p.Code == permissionName);
            var result = Db.RolePermissions.Where(rp => roles.Contains(rp.RoleId))
                .Join(permission, rp => rp.PermissionId, p => p.Id, (rp, p) => p)
                .Any();

            return Task.FromResult(result);
        }

        public PermissionNodeResponse[] GetTreePermissions(int userId = 0)
        {
            IQueryable<Permission> permissionQuery = Db.Permissions;
            if (userId > 0)
            {
                var permissions = Db.UserRoles.Where(ur => ur.UserId == userId)
                             .Join(Db.RolePermissions, ur => ur.RoleId, rp => rp.RoleId, (ur, rp) => rp.PermissionId)
                             .Distinct();
                permissionQuery = permissionQuery.Where(p => permissions.Contains(p.Id));
            }
            var permissionTree = permissionQuery
                .Select(p => new PermissionNodeResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    ParentId = p.ParentId,
                    Path = p.Path,
                    Type = ((PermissionType)p.Type).GetDescription(),
                }).ToList().BuildTree();

            return permissionTree.ToArray();
        }

        public Task<Paginated<UserEntityResponse>> GetPaginatedUsers(int page, int pageSize)
        {
            var response = new Paginated<UserEntityResponse>();

            var query = Db.Users;
            var entities = query.PageByNumber(page, pageSize)
                .Select(i => new UserEntityResponse
                {
                    Id = i.Id,
                    UserName = i.UserName,
                }).ToList();
            var count = query.Count();

            response.Succeed(entities, count);
            return Task.FromResult(response);
        }

        public Task<Wrapped<UserRolesResponse>> GetUserRolesByUserId(int userId)
        {
            var response = new Wrapped<UserRolesResponse>();

            var userEntity = Db.Users.GetById(userId);
            if (userEntity == null)
            {
                response.Fail("User not exist");
            }
            else
            {
                var userRoles = Db.UserRoles.Where(ur => ur.UserId == userId)
                    .Select(ur => ur.RoleId).ToList();
                response.Succeed(new UserRolesResponse
                {
                    Id = userEntity.Id,
                    UserName = userEntity.UserName,
                    Roles = userRoles.ToArray(),
                });
            }

            return Task.FromResult(response);
        }

        public Task<Wrapped<Id>> CreateUser(string userName, string password, int[] roles)
        {
            var response = new Wrapped<Id>();

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var model = UserModel.Create(userName, passwordHash);
            using var trans = Db.Database.BeginTransaction();
            try
            {
                var userEntity = Db.Users.Add(model).Entity;
                Db.SaveChanges();
                var userRoles = roles.Select(roleId => UserRole.Create(userEntity.Id, roleId)).ToList();
                Db.UserRoles.AddRange(userRoles);
                Db.SaveChanges();

                trans.Commit();
                response.Succeed(new Id(userEntity.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                trans.Rollback();
                response.Fail("Failed to create user");
            }

            return Task.FromResult(response);
        }

        public Task<Flag> ModifyUser(int userId, int[] roles)
        {
            var response = new Flag();

            var userEntity = Db.Users.GetById(userId);
            if (userEntity == null)
            {
                response.Fail("User not exist");
            }
            else
            {
                using var trans = Db.Database.BeginTransaction();
                try
                {
                    var oldRoles = Db.UserRoles.Where(ur => ur.UserId == userId)
                        .Select(ur => ur.RoleId).ToList();
                    Db.UserRoles.RemoveRange(oldRoles.Except(roles).ToArray());
                    Db.SaveChanges();
                    var newRoles = roles.Except(oldRoles)
                           .Select(roleId => UserRole.Create(userId, roleId));
                    Db.UserRoles.AddRange(newRoles);
                    Db.SaveChanges();

                    trans.Commit();
                    response.Succeed();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    trans.Rollback();
                    response.Fail("Failed to modify user and roles");
                }
            }

            return Task.FromResult(response);
        }

        public Task<Flag> DeleteUser(int userId)
        {
            var response = new Flag();

            var userEntity = Db.Users.GetById(userId);
            if (userEntity == null)
            {
                response.Fail("User not exist");
            }
            else
            {
                Db.Users.Remove(userEntity);
                Db.SaveChanges();

                response.Succeed();
            }

            return Task.FromResult(response);
        }
    }
}
