using LilySimple.Authorizations;
using LilySimple.Controllers;
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
    /// 用户管理
    /// </summary>
    public class UsersController : RbacAreaControllerBase
    {
        private readonly RbacService _userService;

        public UsersController(RbacService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Permission("user-list")]
        public async Task<ActionResult> GetPaginatedUsers([FromQuery] UserQueryRequest request)
        {
            var result = await _userService.GetPaginatedUsers(request.Page, request.PageSize);
            return Ok(result);
        }

        /// <summary>
        /// 单个用户及其角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserRolesByUserId(id);
            return Ok(result);
        }

        /// <summary>
        /// 添加用户并分配角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Permission("user-create")]
        public async Task<ActionResult> CreateUser([FromBody] UserCreateRequest request)
        {
            var result = await _userService.CreateUser(request.UserName, request.Password, request.Roles);
            return Ok(result);
        }

        /// <summary>
        /// 修改用户并分配角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Permission("user-modify")]
        public async Task<ActionResult> ModifyUser([FromBody] UserModifyRequest request)
        {
            var result = await _userService.ModifyUser(request.Id, request.Roles);
            return Ok(result);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int:min(1)}")]
        [Permission("user-delete")]
        public async Task<ActionResult> DeleteUser([FromRoute] int id)
        {
            var result = await _userService.DeleteUser(id);
            return Ok(result);
        }
    }
}
