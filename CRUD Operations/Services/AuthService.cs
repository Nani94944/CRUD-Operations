using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using CRUD_Operations.Data;
using CRUD_Operations.DTOs.Auth;
using CRUD_Operations.DTOs.Users;
using CRUD_Operations.Interfaces;
using CRUD_Operations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace CRUD_Operations.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthService ( DataContext context , IConfiguration config , IMapper mapper )
        {
            _context = context;
            _config = config;
            _mapper = mapper;
        }

        public async Task<string> Login ( LoginDto loginDto )
        {
            var user = await _context.Users
                .Include ( u => u.Role )
                .FirstOrDefaultAsync ( u => u.Username.ToLower () == loginDto.Username.ToLower () );

            if (user == null) throw new Exception ( "User not found" );

            if (!VerifyPasswordHash ( loginDto.Password , user.PasswordHash , user.PasswordSalt ))
                throw new Exception ( "Wrong password" );

            return CreateToken ( user );
        }

        public async Task<UserDto> Register ( RegisterDto registerDto )
        {
            if (await UserExists ( registerDto.Username ))
                throw new Exception ( "Username already exists" );

            CreatePasswordHash ( registerDto.Password , out byte[] passwordHash , out byte[] passwordSalt );

            var user = new User
            {
                Username = registerDto.Username ,
                Email = registerDto.Email ,
                PasswordHash = passwordHash ,
                PasswordSalt = passwordSalt ,
                RoleId = registerDto.RoleId ,
                CreatedAt = DateTime.UtcNow ,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add ( user );
            await _context.SaveChangesAsync ();

            return _mapper.Map<UserDto> ( user );
        }

        private async Task<bool> UserExists ( string username )
        {
            return await _context.Users.AnyAsync ( u => u.Username.ToLower () == username.ToLower () );
        }

        private void CreatePasswordHash ( string password , out byte[] passwordHash , out byte[] passwordSalt )
        {
            using var hmac = new HMACSHA512 ();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash ( Encoding.UTF8.GetBytes ( password ) );
        }

        private bool VerifyPasswordHash ( string password , byte[] passwordHash , byte[] passwordSalt )
        {
            using var hmac = new HMACSHA512 ( passwordSalt );
            var computedHash = hmac.ComputeHash ( Encoding.UTF8.GetBytes ( password ) );
            return computedHash.SequenceEqual ( passwordHash );
        }

        private string CreateToken ( User user )
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey ( Encoding.UTF8.GetBytes (
                _config.GetSection ( "AppSettings:Token" ).Value ) );

            var creds = new SigningCredentials ( key , SecurityAlgorithms.HmacSha512Signature );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity ( claims ) ,
                Expires = DateTime.Now.AddDays ( 1 ) ,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler ();
            var token = tokenHandler.CreateToken ( tokenDescriptor );
            return tokenHandler.WriteToken ( token );
        }
    }
}