using AutoMapper;
using AutoMapper.QueryableExtensions;
using CRUD_Operations.Data;
using CRUD_Operations.DTOs.Roles;
using CRUD_Operations.Interfaces;
using CRUD_Operations.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Operations.Services
{
    public class RoleService : IRoleService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RoleService ( DataContext context , IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRoles ( )
        {
            return await _context.Roles.ProjectTo<RoleDto> ( _mapper.ConfigurationProvider ).ToListAsync ();
        }

        public async Task<RoleDto> GetRoleById ( int id )
        {
            var role = await _context.Roles.FindAsync ( id );
            if (role == null) throw new Exception ( "Role not found" );
            return _mapper.Map<RoleDto> ( role );
        }

        public async Task<RoleDto> CreateRole ( RoleCreateDto roleCreateDto )
        {
            if (await _context.Roles.AnyAsync ( r => r.Name == roleCreateDto.Name ))
                throw new Exception ( "Role name already exists" );

            var role = _mapper.Map<Role> ( roleCreateDto );
            role.CreatedAt = DateTime.UtcNow;
            role.UpdatedAt = DateTime.UtcNow;

            _context.Roles.Add ( role );
            await _context.SaveChangesAsync ();

            return _mapper.Map<RoleDto> ( role );
        }

        public async Task<RoleDto> UpdateRole ( int id , RoleCreateDto roleCreateDto )
        {
            var role = await _context.Roles.FindAsync ( id );
            if (role == null) throw new Exception ( "Role not found" );

            if (roleCreateDto.Name != role.Name &&
                await _context.Roles.AnyAsync ( r => r.Name == roleCreateDto.Name ))
                throw new Exception ( "Role name already exists" );

            _mapper.Map ( roleCreateDto , role );
            role.UpdatedAt = DateTime.UtcNow;

            _context.Roles.Update ( role );
            await _context.SaveChangesAsync ();

            return _mapper.Map<RoleDto> ( role );
        }

        public async Task<bool> DeleteRole ( int id )
        {
            var role = await _context.Roles.FindAsync ( id );
            if (role == null) throw new Exception ( "Role not found" );

            if (await _context.Users.AnyAsync ( u => u.RoleId == id ))
                throw new Exception ( "Cannot delete role with assigned users" );

            _context.Roles.Remove ( role );
            return await _context.SaveChangesAsync () > 0;
        }
    }
}