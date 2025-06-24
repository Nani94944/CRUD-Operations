using CRUD_Operations.DTOs.Auth;
using CRUD_Operations.DTOs.Users;

namespace CRUD_Operations.Interfaces
{
    public interface IAuthService
    {
        Task<string> Login ( LoginDto loginDto );
        Task<UserDto> Register ( RegisterDto registerDto );
    }
}