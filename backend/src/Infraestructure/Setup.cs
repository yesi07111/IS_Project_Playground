using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Playground.Infraestructure.Data.DbContexts;
using Playground.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Playground.Infraestructure.Configurations;
using Playground.Application.Services;
using Playground.Infraestructure.Services;
using Playground.Application.Factories;
using Playground.Infraestructure.Factories;
using Playground.Application.Repositories;
using Playground.Infraestructure.Repositories;
using Playground.Application.Commands.CleanUp;
using Playground.Application.Queries.Auth.GetRecaptchaSiteKey;
using Playground.Application.Queries.Auth.GetGoogleClientId;

namespace Playground.Infraestructure;

/// <summary>
/// Clase estática para configurar la infraestructura de la aplicación.
/// Proporciona métodos para registrar servicios esenciales como el contexto de base de datos, identidad, y configuraciones específicas.
/// </summary>
public static class Setup
{
    /// <summary>
    /// Configura los servicios de infraestructura necesarios para la aplicación.
    /// Incluye la configuración del contexto de base de datos, identidad de usuario, y servicios personalizados.
    /// </summary>
    /// <param name="builder">La colección de servicios de la aplicación.</param>
    /// <param name="configuration">El administrador de configuración que contiene los parámetros de configuración.</param>
    /// <returns>La colección de servicios con la infraestructura configurada.</returns>
    public static IServiceCollection AddInfraestructure(this IServiceCollection builder, ConfigurationManager configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration), "Configuration cannot be null");
        }

        builder.AddDbContext<DefaultDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Postgres");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'Postgres' is not configured.");
            }

            options.UseNpgsql(connectionString, mig => mig.MigrationsAssembly("Infraestructure"));
        });

        builder.AddIdentity<User, IdentityRole>(options =>
        {
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.AllowedForNewUsers = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
        })
        .AddEntityFrameworkStores<DefaultDbContext>()
        .AddDefaultTokenProviders();

        builder.Configure<JwtConfiguration>(configuration.GetRequiredSection("Jwt"));
        builder.Configure<EmailConfiguration>(configuration.GetRequiredSection("Email"));

        builder.AddHttpClient();

        builder.AddScoped<IJwtGenerator, JwtGenerator>()
               .AddScoped<IDateTimeService, DateTimeService>()
               .AddScoped<IEmailSenderService, EmailSenderService>()
               .AddScoped<IActiveSession, ActiveSession>()
               .AddScoped<IRepositoryFactory, RepositoryFactory>()
               .AddScoped<IUnitOfWork, UnitOfWork>()
               .AddScoped<ICodeGenerator, CodeGenerator>()
               .AddScoped<IConverterService, ConverterService>()
               .AddScoped<IImageService, ImageService>()
               .AddScoped<IRatingService, RatingService>()
               .AddScoped<ICommentsService, CommentsService>()
               .AddScoped<ILoginProvider, LoginProvider>()
               .AddScoped<IAccessFailedService, AccessFailedService>()
               .AddScoped<CleanUpUnverifiedUsersCommandHandler>()
               .AddScoped<GetRecaptchaSiteKeyQueryHandler>()
               .AddScoped<GetGoogleClientIdQueryHandler>()
               .AddScoped<UserManager<User>>();

        return builder;
    }
}