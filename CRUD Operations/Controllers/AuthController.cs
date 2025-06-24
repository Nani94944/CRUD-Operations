using CRUD_Operations.DTOs.Auth;
using CRUD_Operations.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Operations.Controllers
{
    [Route ( "api/[controller]" )]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController ( IAuthService authService )
        {
            _authService = authService;
        }

        [HttpPost ( "login" )]
        public async Task<IActionResult> Login ( LoginDto loginDto )
        {
            var token = await _authService.Login ( loginDto );
            return Ok ( new { token } );
        }

        [HttpPost ( "register" )]
        public async Task<IActionResult> Register ( RegisterDto registerDto )
        {
            var user = await _authService.Register ( registerDto );
            return Ok ( user );
        }
    }
}