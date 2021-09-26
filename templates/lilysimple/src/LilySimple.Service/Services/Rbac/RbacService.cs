using LilySimple.DataStructure.Tree;
using LilySimple.Entities;
using LilySimple.Shared.Enums;
using Microsoft.Extensions.Logging;
using Rise.EfCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilySimple.Services
{
    public partial class ErrorCode
    {
        public const int PermissionNotFound = 41002;
        public const int PermissionCodeOrNameDuplicated = 41003;
        public const int ParentPermissionNotFound = 41004;
        public const int CannotDeletePermissionThatContainsSubPermissions = 41005;
        public const int CannotDeletePermissionThatGrantedToRoles = 41006;
        public const int RoleNotFound = 41007;
        public const int CannotModifyReservedRole = 41008;
        public const int RoleNameDuplicated = 41009;
        public const int CannotDeleteReservedRole = 41010;
        public const int RoleInUse = 41011;
        public const int UserNotFound = 41012;
        public const int UserNameDuplicated = 41013;
    }

    public class RbacService : ServiceBase
    {
        public RbacService()
        {
        }

        public Task<R> GetFullTreePermissions()
        {
            return Task.FromResult(R.List(GetTreePermissions()));
        }

        public void InitializeAdminUser()
        {
            var adminUserName = Configuration["AdminInit:UserName"] ?? "admin";
            var adminPassword = Configuration["AdminInit:Password"] ?? "123456";

            if (Db.Users.Any(u => u.UserName.Equals(adminUserName)))
            {
                Logger.LogInformation("admin account exists, quit init process");
                return;
            }

            var model = new User
            {
                UserName = adminUserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword)
            };
            var entity = Db.Users.Add(model).Entity;
            if (Db.SaveChanges() > 0)
            {
                Logger.LogInformation("admin account has been created.");
            }
        }

        public Task<R> GetPaginatedPermissions(int page, int pageSize)
        {
            var entities = Db.Permissions
                .Count(out var count)
                .PageByNumber(page, pageSize)
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
            return Task.FromResult(R.Page(entities, count));
        }

        public Task<R> GetPermissionById(int id)
        {

            var permissionEntity = Db.Permissions.GetById(id).FirstOrDefault();
            if (permissionEntity == null)
            {
                return Task.FromResult(R.Error(ErrorCode.PermissionNotFound, nameof(ErrorCode.PermissionNotFound)));
            }

            return Task.FromResult(R.Object(new PermissionEntityResponse
            {
                Id = permissionEntity.Id,
                Name = permissionEntity.Name,
                Code = permissionEntity.Code,
                Path = permissionEntity.Path,
                ParentId = permissionEntity.ParentId,
                Type = ((PermissionType)permissionEntity.Type).GetDescription(),
                Sort = permissionEntity.Sort,
            }));
        }

        public Task<R> CreatePermission(string name, string code, string path, int parentId, string type, int sort)
        {
            if (Db.Permissions.Any(p => p.Name == name || p.Code == code))
            {
                return Task.FromResult(R.Error(ErrorCode.PermissionCodeOrNameDuplicated, nameof(ErrorCode.PermissionCodeOrNameDuplicated)));
            }
            if (parentId > 0 && Db.Permissions.GetById(parentId) == null)
            {
                return Task.FromResult(R.Error(ErrorCode.ParentPermissionNotFound, nameof(ErrorCode.ParentPermissionNotFound)));
            }

            var model = Permission.Create(name, code, path, parentId, type.ToEnumValue<PermissionType>(), sort);
            var entity = Db.Permissions.Add(model).Entity;
            Db.SaveChanges();
            return Task.FromResult(R.Object(new PermissionEntityResponse
            {
                Id = entity.Id,
                Code = code,
                Name = name,
            }));
        }

        public Task<R> ModifyPermission(int id, string name, string code, string path, int parentId, string type, int sort)
        {
            var permissionEntity = Db.Permissions.GetById(id).FirstOrDefault();
            if (permissionEntity == null)
            {
                return Task.FromResult(R.Error(ErrorCode.PermissionNotFound, nameof(ErrorCode.PermissionNotFound)));
            }
            if (Db.Permissions.Where(p => p.Id != id).Any(p => p.Name == name || p.Code == code))
            {
                return Task.FromResult(R.Error(ErrorCode.PermissionCodeOrNameDuplicated, nameof(ErrorCode.PermissionCodeOrNameDuplicated)));
            }
            if (parentId > 0 && Db.Permissions.GetById(parentId) == null)
            {
                return Task.FromResult(R.Error(ErrorCode.ParentPermissionNotFound, nameof(ErrorCode.ParentPermissionNotFound)));
            }
            permissionEntity.Modify(name, code, path, parentId, type.ToEnumValue<PermissionType>(), sort);
            Db.SaveChanges();
            return Task.FromResult(R.Ok());
        }

        public Task<R> DeletePermission(int id)
        {
            var permissionEntity = Db.Permissions.GetById(id).FirstOrDefault();
            if (permissionEntity == null)
            {
                return Task.FromResult(R.Error(ErrorCode.PermissionNotFound, nameof(ErrorCode.PermissionNotFound)));
            }
            if (Db.Permissions.Any(p => p.ParentId == id))
            {
                return Task.FromResult(R.Error(ErrorCode.CannotDeletePermissionThatContainsSubPermissions, nameof(ErrorCode.CannotDeletePermissionThatContainsSubPermissions)));
            }
            if (Db.RolePermissions.Any(rp => rp.PermissionId == id))
            {
                return Task.FromResult(R.Error(ErrorCode.CannotDeletePermissionThatGrantedToRoles, nameof(ErrorCode.CannotDeletePermissionThatGrantedToRoles)));
            }
            Db.Permissions.Remove(permissionEntity);
            Db.SaveChanges();
            return Task.FromResult(R.Ok());
        }

        public Task<R> GetPaginatedRoles(int page, int pageSize)
        {
            var query = Db.Roles;
            var entities = query
                .Count(out var count)
                .PageByNumber(page, pageSize)
                .Select(i => new RoleEntityResponse
                {
                    Id = i.Id,
                    Name = i.Name,
                    IsReserved = i.IsReserved,
                }).ToList();
            return Task.FromResult(R.Page(entities, count));
        }

        public Task<R> GetRolePermissionsByRoleId(int roleId)
        {
            var roleEntity = Db.Roles.GetById(roleId).FirstOrDefault();
            if (roleEntity == null)
            {
                return Task.FromResult(R.Error(ErrorCode.RoleNotFound, nameof(ErrorCode.RoleNotFound)));
            }
            var rolePermissions = Db.RolePermissions.Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.PermissionId).ToList();
            return Task.FromResult(R.Object(new RolePermissionsRespose
            {
                Id = roleEntity.Id,
                Name = roleEntity.Name,
                IsReserved = roleEntity.IsReserved,
                Permissions = rolePermissions.ToArray(),
            }));
        }

        public Task<R> CreateRole(string name, int[] permissions)
        {
            if (Db.Roles.Any(r => r.Name == name))
            {
                return Task.FromResult(R.Error(ErrorCode.RoleNameDuplicated, nameof(ErrorCode.RoleNameDuplicated)));
            }
            var roleModel = Role.Create(name);
            permissions = Db.Permissions
                .Where(p => permissions.Contains(p.Id))
                .Select(p => p.Id).ToArray();
            using var trans = Db.Database.BeginTransaction();
            try
            {
                var roleEntity = Db.Roles.Add(roleModel).Entity;
                Db.SaveChanges();
                var rolePermissionModels = permissions
                    .Select(permissionsId => RolePermission.Create(roleEntity.Id, permissionsId)).ToList();
                Db.RolePermissions.AddRange(rolePermissionModels);
                Db.SaveChanges();

                trans.Commit();
                return Task.FromResult(R.Object(roleEntity.Id));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                trans.Rollback();
                throw;
            }
        }

        public Task<R> ModifyRole(int roleId, string name, int[] permissions)
        {
            var roleEntity = Db.Roles.GetById(roleId).FirstOrDefault();
            if (roleEntity == null)
            {
                return Task.FromResult(R.Error(ErrorCode.RoleNotFound, nameof(ErrorCode.RoleNotFound)));
            }
            if (roleEntity.IsReserved)
            {
                return Task.FromResult(R.Error(ErrorCode.CannotModifyReservedRole, nameof(ErrorCode.CannotModifyReservedRole)));
            }
            if (Db.Roles.Where(r => r.Id != roleId).Any(r => r.Name == name))
            {
                return Task.FromResult(R.Error(ErrorCode.RoleNameDuplicated, nameof(ErrorCode.RoleNameDuplicated)));
            }
            permissions = Db.Permissions
                .Where(p => permissions.Contains(p.Id))
                .Select(p => p.Id).ToArray();
            var oldPermissions = Db.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.PermissionId).ToList();
            var newPermissions = permissions.Except(oldPermissions)
                .Select(permissionId => RolePermission.Create(roleId, permissionId));

            using var trans = Db.Database.BeginTransaction();
            try
            {
                roleEntity.Modify(name);
                Db.RolePermissions.RemoveRange(oldPermissions.Except(permissions).ToArray());
                Db.RolePermissions.AddRange(newPermissions);
                Db.SaveChanges();

                trans.Commit();
                return Task.FromResult(R.Ok());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                trans.Rollback();
                throw;
            }
        }

        public Task<R> DeleteRole(int roleId)
        {
            var roleEntity = Db.Roles.GetById(roleId).FirstOrDefault();
            if (roleEntity == null)
            {
                return Task.FromResult(R.Error(ErrorCode.RoleNotFound, nameof(ErrorCode.RoleNotFound)));
            }
            if (roleEntity.IsReserved)
            {
                return Task.FromResult(R.Error(ErrorCode.CannotDeleteReservedRole, nameof(ErrorCode.CannotDeleteReservedRole)));
            }
            if (Db.UserRoles.Any(ur => ur.RoleId == roleId))
            {
                return Task.FromResult(R.Error(ErrorCode.RoleInUse, nameof(ErrorCode.RoleInUse)));
            }
            using var trans = Db.Database.BeginTransaction();
            try
            {
                Db.Roles.Remove(roleEntity);
                var rolePermissions = Db.RolePermissions.Where(rp => rp.RoleId == roleId);
                Db.RolePermissions.RemoveRange(rolePermissions);
                Db.SaveChanges();

                trans.Commit();
                return Task.FromResult(R.Ok());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                trans.Rollback();
                throw;
            }
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
                    Sort = p.Sort,
                }).ToList().BuildSortableTree();

            return permissionTree.ToArray();
        }

        public Task<R> GetPaginatedUsers(int page, int pageSize)
        {
            var adminUserName = Configuration["AdminInit:UserName"] ?? "admin";

            var query = Db.Users.Where(u => u.UserName != adminUserName);
            var entities = query
                .Count(out var count)
                .PageByNumber(page, pageSize)
                .Select(i => new UserEntityResponse
                {
                    Id = i.Id,
                    UserName = i.UserName,
                }).ToList();

            return Task.FromResult(R.Page(entities, count));
        }

        public Task<R> GetUserRolesByUserId(int userId)
        {
            var userEntity = Db.Users.GetById(userId).FirstOrDefault();
            if (userEntity == null)
            {
                return Task.FromResult(R.Error(ErrorCode.UserNotFound, nameof(ErrorCode.UserNotFound)));
            }
            var userRoles = Db.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToList();
            return Task.FromResult(R.Object(new UserRolesResponse
            {
                Id = userEntity.Id,
                UserName = userEntity.UserName,
                Roles = userRoles.ToArray(),
            }));
        }

        public Task<R> CreateUser(string userName, string password, int[] roles)
        {
            if (Db.Users.Any(u => u.UserName == userName))
            {
                return Task.FromResult(R.Error(ErrorCode.UserNameDuplicated, nameof(ErrorCode.UserNameDuplicated)));
            }

            var model = User.Create(userName, BCrypt.Net.BCrypt.HashPassword(password));
            roles = Db.Roles.Where(r => roles.Contains(r.Id))
                .Select(r => r.Id).ToArray();
            using var trans = Db.Database.BeginTransaction();
            try
            {
                var userEntity = Db.Users.Add(model).Entity;
                Db.SaveChanges();

                var userRoles = roles.Select(roleId => UserRole.Create(userEntity.Id, roleId)).ToList();
                Db.UserRoles.AddRange(userRoles);
                Db.SaveChanges();

                trans.Commit();
                return Task.FromResult(R.Ok());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                trans.Rollback();
                throw;
            }
        }

        public Task<R> ModifyUser(int userId, int[] roles)
        {
            var userEntity = Db.Users.GetById(userId).FirstOrDefault();
            if (userEntity == null)
            {
                return Task.FromResult(R.Error(ErrorCode.UserNotFound, nameof(ErrorCode.UserNotFound)));
            }
            var oldRoles = Db.UserRoles.Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId).ToList();
            var newRoles = roles.Except(oldRoles)
                   .Select(roleId => UserRole.Create(userId, roleId));
            using var trans = Db.Database.BeginTransaction();
            try
            {
                userEntity.Modify();
                Db.UserRoles.RemoveRange(oldRoles.Except(roles).ToArray());
                Db.UserRoles.AddRange(newRoles);
                Db.SaveChanges();

                trans.Commit();
                return Task.FromResult(R.Ok());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                trans.Rollback();
                throw;
            }
        }

        public Task<R> DeleteUser(int userId)
        {
            var userEntity = Db.Users.GetById(userId).FirstOrDefault();
            if (userEntity == null)
            {
                return Task.FromResult(R.Error(ErrorCode.UserNotFound, nameof(ErrorCode.UserNotFound)));
            }

            using var trans = Db.Database.BeginTransaction();
            try
            {
                Db.Users.Remove(userEntity);
                Db.SaveChanges();

                Db.UserRoles.RemoveRange(Db.UserRoles.Where(ur => ur.UserId == userId));
                Db.SaveChanges();
                trans.Commit();

                return Task.FromResult(R.Ok());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                trans.Rollback();
                throw;
            }
        }
    }
}
