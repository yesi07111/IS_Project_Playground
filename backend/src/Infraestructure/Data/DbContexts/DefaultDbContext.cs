using Playground.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Playground.Domain.Entities;

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

    /// <summary>
    /// Conjunto de datos para las reseñas.
    /// </summary>
    public DbSet<Review> Review { get; set; }

    /// <summary>
    /// Conjunto de datos para las reservas.
    /// </summary>
    public DbSet<Reservation> Reservation { get; set; }
}