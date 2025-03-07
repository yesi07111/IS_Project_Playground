using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Playground.WebApi;

/// <summary>
/// Clase estática para configurar el esquema de seguridad y roles en la aplicación.
/// </summary>
public static class Setup
{
    /// <summary>
    /// Configura el esquema de seguridad para la autenticación y autorización en la aplicación.
    /// </summary>
    /// <param name="services">La colección de servicios de la aplicación.</param>
    /// <param name="configuration">El administrador de configuración que contiene los parámetros de configuración.</param>
    /// <returns>La colección de servicios con el esquema de seguridad configurado.</returns>
    public static IServiceCollection AddSecuritySchema(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opt =>
        {
            opt.RequireHttpsMetadata = false;
            opt.SaveToken = true;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"]
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
            options.AddPolicy("RequireEducatorRole", policy => policy.RequireRole("Educator"));
            options.AddPolicy("RequireParentRole", policy => policy.RequireRole("Parent"));
        });

        services.AddCors(opt =>
        {
            opt.AddPolicy("AllowLocalhost", x =>
            {
                x.WithOrigins("localhost", "127.0.0.1");
                x.AllowAnyMethod();
                x.AllowAnyHeader();
            });
        });

        return services;
    }

}