using Playground.Application.Repositories;
using Playground.Infraestructure.Data.DbContexts;

namespace Playground.Infraestructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DefaultDbContext _context;

        public UnitOfWork(DefaultDbContext context)
        {
            _context = context;
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}