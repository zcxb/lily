using LilySimple.Authorizations;
using LilySimple.Models.Role;
using LilySimple.Services.Privilege;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilySimple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : BizControllerBase
    {
        private readonly PrivilegeService _privilegeService;

        public RolesController(PrivilegeService privilegeService)
        {
            _privilegeService = privilegeService;
        }

        /// <summary>
        /// 创建角色
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
        /// 修改角色
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
        /// 删除角色
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
