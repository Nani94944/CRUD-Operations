using CRUD_Operations.DTOs.Users;
using CRUD_Operations.Helpers;

namespace CRUD_Operations.Interfaces
{
    public interface IUserService
    {
        Task<PagedList<UserDto>> GetAllUsers ( UserParams userParams );
        Task<UserDto> GetUserById ( int id );
        Task<UserDto> CreateUser ( UserCreateDto userCreateDto );
        Task<UserDto> UpdateUser ( int id , UserUpdateDto userUpdateDto );
        Task<bool> DeleteUser ( int id );
    }
}