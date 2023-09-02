using Antopia.Infrastructure.EmailServices;
using Antopia.Persistence.Commands.LoginCommands;
using Antopia.Persistence.Commands.PublicationCommands;
using Antopia.Persistence.Commands.UserCommands;
using Antopia.Persistence.ImageService;
using Antopia.Persistence.Queries.LoginQueries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Antopia.API.Application
{
    public static class StartupSetup
    {
        public static IServiceCollection AddStartupSetup(this IServiceCollection service, IConfiguration configuration)
        {
            // Autorizacion Services
            service.AddTransient<IAutorizacionService, AutorizacionService>();

            // Email Services
            service.AddTransient<IEmailServices, EmailServices>();

            // Queries Persistance Services
            service.AddTransient<ILoginQueries, LoginQueries>();

            // Commands Persistance Services
            service.AddTransient<ILoginCommands, LoginCommands>();
            service.AddTransient<IUserCommands, UserCommands>();
            service.AddTransient<IPublicationCommands, PublicationCommands>();
            service.AddScoped<IImageService, ImageService>();

            // Authentication Services
            var key = configuration.GetValue<string>("JwtSettings:key");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            service.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            return service;
        }
    }
}
