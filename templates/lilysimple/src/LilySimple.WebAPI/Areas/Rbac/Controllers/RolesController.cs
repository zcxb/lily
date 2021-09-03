using LilySimple.Authorizations;
using LilySimple.QueryModels.Rbac;
using LilySimple.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilySimple.Areas.Rbac.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class RolesController : RbacAreaControllerBase
    {
        private readonly RbacService _privilegeService;

        public RolesController(RbacService privilegeService)
        {
            _privilegeService = privilegeService;
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Permission("role-list")]
        public async Task<ActionResult> GetPaginatedRoles([FromQuery] RoleQueryRequest request)
        {
            var result = await _privilegeService.GetPaginatedRoles(request.Page, request.PageSize);
            return Ok(result);
        }

        /// <summary>
        /// 单个角色及其权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult> GetRoleById(int id)
        {
            var result = await _privilegeService.GetRolePermissionsByRoleId(id);
            return Ok(result);
        }

        /// <summary>
        /// 创建角色并分配权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Permission("role-create")]
        public async Task<ActionResult> CreateRole([FromBody] RoleCreateRequest request)
        {
            var result = await _privilegeService.CreateRole(request.Name, request.Permissions);
            return Ok(result);
        }

        /// <summary>
        /// 修改角色并分配权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Permission("role-modify")]
        public async Task<ActionResult> ModifyRole([FromBody] RoleModifyRequest request)
        {
            var result = await _privilegeService.ModifyRole(request.Id, request.Name, request.Permissions);
            return Ok(result);
        }

        /// <summary>
        /// 删除角色及其权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int:min(1)}")]
        [Permission("role-delete")]
        public async Task<ActionResult> DeleteRole([FromRoute] int id)
        {
            var result = await _privilegeService.DeleteRole(id);
            return Ok(result);
        }
    }
}
