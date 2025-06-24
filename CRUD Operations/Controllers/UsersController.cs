using CRUD_Operations.DTOs.Users;
using CRUD_Operations.Helpers;
using CRUD_Operations.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Operations.Controllers
{
    [Authorize]
    [Route ( "api/[controller]" )]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController ( IUserService userService )
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers ( [FromQuery] UserParams userParams )
        {
            var users = await _userService.GetAllUsers ( userParams );
            Response.AddPaginationHeader ( users.CurrentPage , users.PageSize ,
                users.TotalCount , users.TotalPages );
            return Ok ( users );
        }

        [HttpGet ( "{id}" )]
        public async Task<IActionResult> GetUserById ( int id )
        {
            var user = await _userService.GetUserById ( id );
            return Ok ( user );
        }

        [Authorize ( Roles = "Admin" )]
        [HttpPost]
        public async Task<IActionResult> CreateUser ( UserCreateDto userCreateDto )
        {
            var user = await _userService.CreateUser ( userCreateDto );
            return CreatedAtAction ( nameof ( GetUserById ) , new { id = user.Id } , user );
        }

        [HttpPut ( "{id}" )]
        public async Task<IActionResult> UpdateUser ( int id , UserUpdateDto userUpdateDto )
        {
            var user = await _userService.UpdateUser ( id , userUpdateDto );
            return Ok ( user );
        }

        [Authorize ( Roles = "Admin" )]
        [HttpDelete ( "{id}" )]
        public async Task<IActionResult> DeleteUser ( int id )
        {
            await _userService.DeleteUser ( id );
            return NoContent ();
        }
    }
}