using AutoMapper;
using AutoMapper.QueryableExtensions;
using CRUD_Operations.Data;
using CRUD_Operations.DTOs.Users;
using CRUD_Operations.Helpers;
using CRUD_Operations.Interfaces;
using CRUD_Operations.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CRUD_Operations.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserService ( DataContext context , IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<UserDto>> GetAllUsers ( UserParams userParams )
        {
            var query = _context.Users
                .Include ( u => u.Role )
                .AsQueryable ();

            if (!string.IsNullOrEmpty ( userParams.SearchTerm ))
            {
                query = query.Where ( u =>
                    u.Username.Contains ( userParams.SearchTerm ) ||
                    u.Email.Contains ( userParams.SearchTerm ) );
            }

            query = userParams.OrderBy switch
            {
                "username" => query.OrderBy ( u => u.Username ),
                "createdAt" => query.OrderByDescending ( u => u.CreatedAt ),
                _ => query.OrderBy ( u => u.Id )
            };

            return await PagedList<UserDto>.CreateAsync (
                query.ProjectTo<UserDto> ( _mapper.ConfigurationProvider ).AsNoTracking () ,
                userParams.PageNumber ,
                userParams.PageSize );
        }

        public async Task<UserDto> GetUserById ( int id )
        {
            var user = await _context.Users
                .Include ( u => u.Role )
                .FirstOrDefaultAsync ( u => u.Id == id );

            if (user == null) throw new Exception ( "User not found" );

            return _mapper.Map<UserDto> ( user );
        }

        public async Task<UserDto> CreateUser ( UserCreateDto userCreateDto )
        {
            if (await _context.Users.AnyAsync ( u => u.Username == userCreateDto.Username ))
                throw new Exception ( "Username already exists" );

            if (await _context.Users.AnyAsync ( u => u.Email == userCreateDto.Email ))
                throw new Exception ( "Email already in use" );

            var user = _mapper.Map<User> ( userCreateDto );

            using var hmac = new HMACSHA512 ();
            user.PasswordSalt = hmac.Key;
            user.PasswordHash = hmac.ComputeHash ( Encoding.UTF8.GetBytes ( userCreateDto.Password ) );
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Add ( user );
            await _context.SaveChangesAsync ();

            return _mapper.Map<UserDto> ( user );
        }

        public async Task<UserDto> UpdateUser ( int id , UserUpdateDto userUpdateDto )
        {
            var user = await _context.Users.FindAsync ( id );
            if (user == null) throw new Exception ( "User not found" );

            if (userUpdateDto.Username != user.Username &&
                await _context.Users.AnyAsync ( u => u.Username == userUpdateDto.Username ))
                throw new Exception ( "Username already taken" );

            if (userUpdateDto.Email != user.Email &&
                await _context.Users.AnyAsync ( u => u.Email == userUpdateDto.Email ))
                throw new Exception ( "Email already in use" );

            _mapper.Map ( userUpdateDto , user );
            user.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrEmpty ( userUpdateDto.Password ))
            {
                using var hmac = new HMACSHA512 ();
                user.PasswordSalt = hmac.Key;
                user.PasswordHash = hmac.ComputeHash ( Encoding.UTF8.GetBytes ( userUpdateDto.Password ) );
            }

            _context.Users.Update ( user );
            await _context.SaveChangesAsync ();

            return _mapper.Map<UserDto> ( user );
        }

        public async Task<bool> DeleteUser ( int id )
        {
            var user = await _context.Users.FindAsync ( id );
            if (user == null) throw new Exception ( "User not found" );

            _context.Users.Remove ( user );
            return await _context.SaveChangesAsync () > 0;
        }
    }
}