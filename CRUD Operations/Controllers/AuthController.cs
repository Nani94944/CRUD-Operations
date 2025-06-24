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
            try
            {
                var token = await _authService.Login ( loginDto );
                return LocalizedOk( "LoginSuccess" , new { token } );
            }
            catch (Exception ex)
            {
                return LocalizedBadRequest( ex.Message );
            }
        }

        [HttpPost ( "register" )]
        public async Task<IActionResult> Register ( RegisterDto registerDto )
        {
            try
            {
                var user = await _authService.Register ( registerDto );
                return LocalizedOk( "RegistrationSuccess" , user );
            }
            catch (Exception ex)
            {
                return LocalizedBadRequest( ex.Message );
            }
        }
    }
}