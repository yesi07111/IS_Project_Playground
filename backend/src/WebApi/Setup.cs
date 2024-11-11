using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Playground.WebApi;

public static class Setup
{
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

        services.AddAuthorization();

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