using LilySimple.Authorizations;
using LilySimple.Services.Privilege;
using LilySimple.Shared.Consts;
using LilySimple.ViewModels.Permission;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilySimple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : BizControllerBase
    {
        private readonly PrivilegeService _privilegeService;

        public PermissionsController(PrivilegeService privilegeService)
        {
            _privilegeService = privilegeService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="format">数据格式：tree/list</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetPermissions([FromQuery] PermissionQueryRequest request, string format)
        {
            switch (format)
            {
                case ApiResponseFormat.Tree:
                    var tree = await _privilegeService.GetPermissionTree();
                    return Ok(tree);
                default:
                    var list = await _privilegeService.GetPermissionList(request.Page, request.PageSize);
                    return Ok(list);
            }
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task GetPermissionById([FromRoute] int id)
        {

        }

        [HttpPost]
        public async Task<ActionResult> CreatePermission([FromBody] PermissionCreateRequest request)
        {
            var result = await _privilegeService.CreatePermission(request.Name, request.Code, request.Path, request.Type, request.ParentId);
            return Ok(result);
        }

        [HttpPut]
        public async Task ModifyPermission([FromBody] PermissionModifyRequest request)
        {

        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task DeletePermission([FromRoute] int id)
        {

        }
    }
}
