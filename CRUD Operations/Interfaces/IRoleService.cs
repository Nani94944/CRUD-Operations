using CRUD_Operations.DTOs.Roles;

namespace CRUD_Operations.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRoles ( );
        Task<RoleDto> GetRoleById ( int id );
        Task<RoleDto> CreateRole ( RoleCreateDto roleCreateDto );
        Task<RoleDto> UpdateRole ( int id , RoleCreateDto roleCreateDto );
        Task<bool> DeleteRole ( int id );
    }
}