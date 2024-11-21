using Microsoft.EntityFrameworkCore;
using Playground.Application.Repositories;
using Playground.Domain.Specifications.BaseSpecifications;
using Playground.Infraestructure.Data.DbContexts;

namespace Playground.Infraestructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DefaultDbContext _context;

        public Repository(DefaultDbContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id) ?? null;
        }

        public async Task<IEnumerable<T>> GetBySpecificationAsync(ISpecification<T> specification)
        {
            return await _context.Set<T>().Where(entity => specification.IsSatisfiedBy(entity)).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}