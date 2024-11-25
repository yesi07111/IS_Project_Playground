using Microsoft.EntityFrameworkCore;
using Playground.Application.Repositories;
using Playground.Domain.Entities.Auth;
using Playground.Domain.Specifications.BaseSpecifications;
using Playground.Infraestructure.Data.DbContexts;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            Expression<Func<T, bool>> expression = specification.ToExpression();
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllAdminsAsync()
        {
            return await _context.Users
                .Where(u => _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && _context.Roles
                        .Any(r => r.Id == ur.RoleId && r.Name == "Admin")))
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllEducatorsAsync()
        {
            return await _context.Users
                .Where(u => _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && _context.Roles
                        .Any(r => r.Id == ur.RoleId && r.Name == "Educator")))
                .ToListAsync();
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

        public void MarkDeleted(User entity)
        {
            string dateSuffix = "_deleted_" + DateTime.UtcNow.ToString("dd_MM_yyyy_HH_mm_ss_fff");

            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;

            entity.UserName += dateSuffix;
            entity.FirstName += dateSuffix;
            entity.LastName += dateSuffix;
            entity.Email += dateSuffix;
            entity.NormalizedUserName += dateSuffix;
            entity.NormalizedEmail += dateSuffix;

            _context.Set<User>().Update(entity);
        }
    }
}