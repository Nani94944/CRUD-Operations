using CRUD_Operations.Data;
using CRUD_Operations.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder ( args );
builder.Services.AddLocalization ( options => options.ResourcesPath = "Resources" );

builder.Services.Configure<RequestLocalizationOptions> ( options =>
{
    var supportedCultures = new[] { "en" , "ar" };
    options.SetDefaultCulture ( "en" )
           .AddSupportedCultures ( supportedCultures )
           .AddSupportedUICultures ( supportedCultures );

    options.ApplyCurrentCultureToResponseHeaders = true;
} );

// Add to middleware pipeline (before authorization)
// Add services to the container.
builder.Services.AddControllers ();
builder.Services.AddApplicationServices ( builder.Configuration );
builder.Services.AddAuthentication ( JwtBearerDefaults.AuthenticationScheme )
    .AddJwtBearer ( options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true ,
            IssuerSigningKey = new SymmetricSecurityKey ( Encoding.UTF8
                .GetBytes ( builder.Configuration.GetSection ( "AppSettings:Token" ).Value ) ) ,
            ValidateIssuer = false ,
            ValidateAudience = false
        };
    } );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer ();
builder.Services.AddSwaggerGen ();

var app = builder.Build ();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment ())
{
    app.UseSwagger ();
    app.UseSwaggerUI ();
}

app.UseRequestLocalization ();
app.UseMiddleware<ExceptionMiddleware> ();

app.UseHttpsRedirection ();

app.UseAuthentication ();
app.UseAuthorization ();

app.MapControllers ();

// Seed database
using var scope = app.Services.CreateScope ();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext> ();
    await context.Database.MigrateAsync ();
    await Seed.SeedRoles ( context );
    await Seed.SeedAdminUser ( context , builder.Configuration );
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>> ();
    logger.LogError ( ex , "An error occurred during migration" );
}

app.Run ();