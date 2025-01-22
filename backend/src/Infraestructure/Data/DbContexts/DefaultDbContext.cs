using Playground.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Playground.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Playground.Infraestructure.Data.DbContexts;

/// <summary>
/// Contexto de base de datos predeterminado que extiende <see cref="IdentityDbContext{TUser}"/>.
/// Gestiona las entidades de la aplicación y proporciona acceso a las tablas de la base de datos.
/// </summary>
public class DefaultDbContext : IdentityDbContext<User>
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DefaultDbContext"/>.
    /// </summary>
    /// <param name="options">Opciones para configurar el contexto de base de datos.</param>
    public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options) { }

    /// <summary>
    /// Conjunto de datos para las actividades.
    /// </summary>
    public DbSet<Domain.Entities.Activity> Activity { get; set; }

    /// <summary>
    /// Conjunto de datos para las instalaciones.
    /// </summary>
    public DbSet<Facility> Facility { get; set; }

    /// <summary>
    /// Conjunto de datos para los recursos.
    /// </summary>
    public DbSet<Resource> Resource { get; set; }

    public DbSet<ResourceDate> ResourceDate { get; set; }

    /// <summary>
    /// Conjunto de datos para las reseñas.
    /// </summary>
    public DbSet<Review> Review { get; set; }

    /// <summary>
    /// Conjunto de datos para las reservas.
    /// </summary>
    public DbSet<Reservation> Reservation { get; set; }

    /// <summary>
    /// Conjunto de datos para los roles.
    /// </summary>
    public DbSet<Rol> Rol { get; set; }


    /// <summary>
    /// Conjunto de datos para las imágenes del perfil de usuario.
    /// </summary>
    public DbSet<UserProfileImages> UserProfileImages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Establecer el esquema predeterminado
        builder.HasDefaultSchema("public");

        // Ignorar las tablas de roles y claims
        builder.Ignore<IdentityRole>();
        builder.Ignore<IdentityUserRole<string>>();
        builder.Ignore<IdentityUserClaim<string>>();
        builder.Ignore<IdentityRoleClaim<string>>();

        builder.Entity<User>()
            .HasOne(u => u.Rol)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RolId) // Usa RolId como clave foránea
            .IsRequired();
    }

}