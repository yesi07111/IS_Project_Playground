using Playground.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Playground.Domain.Entities;

namespace Playground.Infraestructure.Data.DbContexts;

public class DefaultDbContext : IdentityDbContext<User>
{
    public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options) { }
    public DbSet<Domain.Entities.Activity> Activitie { get; set; }
    public DbSet<Facility> Facilitie { get; set; }
    public DbSet<Resource> Resource { get; set; }
    public DbSet<Review> Review { get; set; }
    public DbSet<Reservation> Reservation { get; set; }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);
    //     // Configuraciones adicionales si es necesario
    // }
}