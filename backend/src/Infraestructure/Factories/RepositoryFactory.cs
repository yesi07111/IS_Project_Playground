using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Infraestructure.Data.DbContexts;
using Playground.Infraestructure.Repositories;

namespace Playground.Infraestructure.Factories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly DefaultDbContext _context;

        public RepositoryFactory(DefaultDbContext context)
        {
            _context = context;
        }

        public IRepository<T> CreateRepository<T>() where T : class
        {
            return new Repository<T>(_context);
        }
    }
}