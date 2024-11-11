using Playground.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Playground.Infraestructure.Data.DbContexts;

public class DefaultDbContext : IdentityDbContext<User>
{
    public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options) { }
}