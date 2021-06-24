using LilySimple.Authorizations;
using LilySimple.Services.Rbac;
using LilySimple.Shared.Consts;
using LilySimple.Models.Permission;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilySimple.Areas.Rbac.Controllers
{
    /// <summary>
    /// 权限管理
    /// </summary>
    public class PermissionsController : RbacAreaControllerBase
    {
        private readonly RbacService _privilegeService;

        public PermissionsController(RbacService privilegeService)
        {
            _privilegeService = privilegeService;
        }

        /// <summary>
        /// 权限列表/树
        /// </summary>
        /// <param name="request"></param>
        /// <param name="format">数据格式：tree/list</param>
        /// <returns></returns>
        [HttpGet]
        [Permission("permission-list")]
        public async Task<ActionResult> GetPermissions([FromQuery] PermissionQueryRequest request, string format)
        {
            switch (format)
            {
                case ApiResponseFormat.Tree:
                    var tree = await _privilegeService.GetFullTreePermissions();
                    return Ok(tree);
                default:
                    var list = await _privilegeService.GetPaginatedPermissions(
                        request.Page, request.PageSize);
                    return Ok(list);
            }
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult> GetPermissionById([FromRoute] int id)
        {
            var result = await _privilegeService.GetPermissionById(id);
            return Ok(result);
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Permission("permission-create")]
        public async Task<ActionResult> CreatePermission([FromBody] PermissionCreateRequest request)
        {
            var result = await _privilegeService.CreatePermission(
                request.Name, request.Code, request.Path, request.ParentId, request.Type, request.Sort);
            return Ok(result);
        }

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Permission("permission-modify")]
        public async Task<ActionResult> ModifyPermission([FromBody] PermissionModifyRequest request)
        {
            var result = await _privilegeService.ModifyPermission(request.Id,
                request.Name, request.Code, request.Path, request.ParentId, request.Type, request.Sort);
            return Ok(result);
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int:min(1)}")]
        [Permission("permission-delete")]
        public async Task<ActionResult> DeletePermission([FromRoute] int id)
        {
            var result = await _privilegeService.DeletePermission(id);
            return Ok(result);
        }
    }
}
