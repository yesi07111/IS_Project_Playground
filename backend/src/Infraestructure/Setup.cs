using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Playground.Infraestructure.Data.DbContexts;
using Playground.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Playground.Infraestructure.Configurations;
using Playground.Application.Services;
using Playground.Infraestructure.Services;

namespace Playground.Infraestructure;

public static class Setup
{
    public static IServiceCollection AddInfraestructure(this IServiceCollection builder, ConfigurationManager configuration)
    {
        builder.AddDbContext<DefaultDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Postgres"),
                              mig => mig.MigrationsAssembly("Infraestructure"));
        });


        builder.AddDefaultIdentity<User>(opt => { opt.SignIn.RequireConfirmedEmail = true; })
             .AddRoles<IdentityRole>()
             .AddEntityFrameworkStores<DefaultDbContext>();

        builder.Configure<JwtConfiguration>(configuration.GetRequiredSection("Jwt"));
        builder.Configure<EmailConfiguration>(configuration.GetRequiredSection("Email"));

        builder.AddScoped<IJwtGenerator, JwtGenerator>()
               .AddScoped<IDateTimeService, DateTimeService>()
               .AddScoped<IEmailSenderService, EmailSenderService>()
               .AddScoped<IActiveSession, ActiveSession>()
               .AddScoped<UserManager<User>>();


        return builder;
    }
}