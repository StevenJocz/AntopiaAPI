using Antopia.Infrastructure.EmailServices;
using Antopia.Persistence.Commands.ColoniaCommands;
using Antopia.Persistence.Commands.DiaryCommands;
using Antopia.Persistence.Commands.LoginCommands;
using Antopia.Persistence.Commands.NotificacionCommands;
using Antopia.Persistence.Commands.PublicationCommands;
using Antopia.Persistence.Commands.UserCommands;
using Antopia.Persistence.ImageService;
using Antopia.Persistence.Queries.ColoniaQueries;
using Antopia.Persistence.Queries.DiaryQueries;
using Antopia.Persistence.Queries.LoginQueries;
using Antopia.Persistence.Queries.NotificacionQueries;
using Antopia.Persistence.Queries.PublicationQueries;
using Antopia.Persistence.Queries.UserQueries;
using Antopia.Persistence.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
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
            service.AddTransient<IPublicationQueries, PublicationQueries>();
            service.AddTransient<IUserQueries, UserQueries>();
            service.AddTransient<IColoniaQueries, ColoniaQueries>();
            service.AddTransient<IDiaryQueries, DiaryQueries>();
            service.AddTransient<INotificacionQueries, NotificacionQueries>();

            // Commands Persistance Services
            service.AddTransient<ILoginCommands, LoginCommands>();
            service.AddTransient<IUserCommands, UserCommands>();
            service.AddTransient<IPublicationCommands, PublicationCommands>();
            service.AddTransient<IColoniaCommands, ColoniaCommands>();
            service.AddTransient<IDiaryCommands, DiaryCommands>();
            service.AddTransient<INotificacionCommands, NotificacionCommands>();

            // Imagen Persistance Services
            service.AddScoped<IImageService, ImageService>();

            // Utilidades Persistance Service IUtilidades
            service.AddScoped<IUtilidades, Utilidades>();

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
