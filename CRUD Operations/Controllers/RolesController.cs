using CRUD_Operations.DTOs.Roles;
using CRUD_Operations.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Operations.Controllers
{
    [Authorize ( Roles = "Admin" )]
    [Route ( "api/[controller]" )]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController ( IRoleService roleService )
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles ( )
        {
            var roles = await _roleService.GetAllRoles ();
            return Ok ( roles );
        }

        [HttpGet ( "{id}" )]
        public async Task<IActionResult> GetRoleById ( int id )
        {
            var role = await _roleService.GetRoleById ( id );
            return Ok ( role );
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole ( RoleCreateDto roleCreateDto )
        {
            var role = await _roleService.CreateRole ( roleCreateDto );
            return CreatedAtAction ( nameof ( GetRoleById ) , new { id = role.Id } , role );
        }

        [HttpPut ( "{id}" )]
        public async Task<IActionResult> UpdateRole ( int id , RoleCreateDto roleCreateDto )
        {
            var role = await _roleService.UpdateRole ( id , roleCreateDto );
            return Ok ( role );
        }

        [HttpDelete ( "{id}" )]
        public async Task<IActionResult> DeleteRole ( int id )
        {
            await _roleService.DeleteRole ( id );
            return NoContent ();
        }
    }
}