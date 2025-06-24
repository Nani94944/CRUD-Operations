using AutoMapper;
using CRUD_Operations.DTOs.Roles;
using CRUD_Operations.DTOs.Users;
using CRUD_Operations.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CRUD_Operations.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles ( )
        {
            // User mappings
            CreateMap<User , UserDto> ()
                .ForMember ( dest => dest.RoleName , opt => opt.MapFrom ( src => src.Role.Name ) );
            CreateMap<UserCreateDto , User> ();
            CreateMap<UserUpdateDto , User> ();

            // Role mappings
            CreateMap<Role , RoleDto> ();
            CreateMap<RoleCreateDto , Role> ();
        }
    }
}