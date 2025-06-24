using CRUD_Operations.Data;
using CRUD_Operations.Interfaces;
using CRUD_Operations.Services;
using Microsoft.EntityFrameworkCore;
using CRUD_Operations.Helpers;


namespace CRUD_Operations.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices ( this IServiceCollection services , IConfiguration config )
        {
            services.AddDbContext<DataContext> ( options =>
            {
                options.UseSqlServer ( config.GetConnectionString ( "DefaultConnection" ) );
            } );

            services.AddScoped<IAuthService , AuthService> ();
            services.AddScoped<IUserService , UserService> ();
            services.AddScoped<IRoleService , RoleService> ();
            services.AddAutoMapper ( typeof ( AutoMapperProfiles ).Assembly );

            return services;
        }
    }
}