using CRUD_Operations.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CRUD_Operations.Data
{
    public static class Seed
    {
        public static async Task SeedRoles ( DataContext context )
        {
            if (await context.Roles.AnyAsync ()) return;

            var roles = new List<Role>
            {
                new Role { Name = "Admin", Description = "Administrator with full access" },
                new Role { Name = "User", Description = "Standard user with limited access" }
            };

            await context.Roles.AddRangeAsync ( roles );
            await context.SaveChangesAsync ();
        }

        public static async Task SeedAdminUser ( DataContext context , IConfiguration config )
        {
            if (await context.Users.AnyAsync ()) return;

            var adminRole = await context.Roles.FirstOrDefaultAsync ( r => r.Name == "Admin" );
            if (adminRole == null) return;

            using var hmac = new HMACSHA512 ();

            var adminUser = new User
            {
                Username = "admin" ,
                Email = "admin@example.com" ,
                PasswordHash = hmac.ComputeHash ( Encoding.UTF8.GetBytes ( config.GetSection ( "AppSettings:AdminPassword" ).Value ) ) ,
                PasswordSalt = hmac.Key ,
                RoleId = adminRole.Id ,
                CreatedAt = DateTime.UtcNow ,
                UpdatedAt = DateTime.UtcNow
            };

            await context.Users.AddAsync ( adminUser );
            await context.SaveChangesAsync ();
        }
    }
}