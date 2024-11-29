using Playground.Domain.Entities.Auth;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Application.Repositories
{
        public interface IRepository<T>
        {
                Task<T?> GetByIdAsync(string id);
                Task<IEnumerable<T>> GetBySpecificationAsync(ISpecification<T> specification);
                Task<IEnumerable<T>> GetAllAsync();
                Task<IEnumerable<User>> GetAllAdminsAsync();
                Task<IEnumerable<User>> GetAllEducatorsAsync();
                Task<IEnumerable<User>> GetAllParentsAsync();
                Task AddAsync(T entity);
                void Update(T entity);
                void Delete(T entity);
                void MarkDeleted(User entity);
        }
}