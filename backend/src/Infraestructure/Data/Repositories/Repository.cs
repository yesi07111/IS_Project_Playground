using Microsoft.EntityFrameworkCore;
using Playground.Application.Repositories;
using Playground.Domain.Entities;
using Playground.Domain.Entities.Auth;
using Playground.Domain.Specifications.BaseSpecifications;
using Playground.Infraestructure.Data.DbContexts;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Playground.Infraestructure.Repositories
{
    /// <summary>
    /// Repositorio genérico para realizar operaciones CRUD en la base de datos.
    /// Proporciona métodos para obtener, agregar, actualizar y eliminar entidades.
    /// </summary>
    /// <typeparam name="T">El tipo de entidad gestionada por el repositorio.</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DefaultDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Repository{T}"/>.
        /// </summary>
        /// <param name="context">El contexto de base de datos utilizado para las operaciones.</param>
        public Repository(DefaultDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene una entidad por su identificador.
        /// </summary>
        /// <param name="id">El identificador de la entidad, string.</param>
        /// <returns>La entidad encontrada o null si no existe.</returns>
        public async Task<T?> GetByIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id) ?? null;
        }

        /// <summary>
        /// Obtiene una entidad por su identificador, incluyendo múltiples propiedades de navegación.
        /// </summary>
        /// <param name="id">El identificador de la entidad, string.</param>
        /// <param name="includes">Funciones para incluir propiedades de navegación y subpropiedades.</param>
        /// <returns>La entidad encontrada o null si no existe.</returns>
        public async Task<T?> GetByIdAsync(string id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<string>(e, "Id") == id);
        }

        /// <summary>
        /// Obtiene una entidad por su identificador.
        /// </summary>
        /// <param name="id">El identificador de la entidad, Guid.</param>
        /// <returns>La entidad encontrada o null si no existe.</returns>
        public async Task<T?> GetByIdAsync(Guid id)
        {

            return await _context.Set<T>().FindAsync(id) ?? null;
        }

        /// <summary>
        /// Obtiene una entidad por su identificador, incluyendo múltiples propiedades de navegación.
        /// </summary>
        /// <param name="id">El identificador de la entidad, Guid.</param>
        /// <param name="includes">Funciones para incluir propiedades de navegación y subpropiedades.</param>
        /// <returns>La entidad encontrada o null si no existe.</returns>
        public async Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        /// <summary>
        /// Obtiene entidades que cumplen con una especificación dada.
        /// </summary>
        /// <param name="specification">La especificación que deben cumplir las entidades.</param>
        /// <returns>Una colección de entidades que cumplen con la especificación.</returns>
        public async Task<IEnumerable<T>> GetBySpecificationAsync(ISpecification<T> specification)
        {
            Expression<Func<T, bool>> expression = specification.ToExpression();
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        /// <summary>
        /// Obtiene entidades que cumplen con una especificación dada, incluyendo múltiples propiedades de navegación.
        /// </summary>
        /// <param name="specification">La especificación que deben cumplir las entidades.</param>
        /// <param name="includeExpressions">Funciones para incluir propiedades de navegación y subpropiedades.</param>
        /// <returns>Una colección de entidades que cumplen con la especificación.</returns>
        public async Task<IEnumerable<T>> GetBySpecificationAsync(ISpecification<T> specification, params Expression<Func<T, object>>[] includeExpressions)
        {
            IQueryable<T> query = _context.Set<T>().Where(specification.ToExpression());

            foreach (var includeExpression in includeExpressions)
            {
                query = query.Include(includeExpression);
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Obtiene entidades que cumplen con una especificación dada, y una especificación adicional para una propiedad de navegación.
        /// </summary>
        /// <typeparam name="TProperty">El tipo de la propiedad de navegación.</typeparam>
        /// <param name="specification">La especificación que deben cumplir las entidades.</param>
        /// <param name="navigationSpecification">La especificación que deben cumplir las entidades de la propiedad de navegación.</param>
        /// <param name="navigationProperty">Función para acceder a la propiedad de navegación.</param>
        /// <param name="includeExpressions">Funciones para incluir propiedades de navegación y subpropiedades.</param>
        /// <returns>Una tarea que representa la operación asincrónica, con una colección de entidades que cumplen con la especificación.</returns>
        public async Task<IEnumerable<T>> GetBySpecificationAsync<TProperty>(
        ISpecification<T> specification,
        ISpecification<TProperty> navigationSpecification,
        Expression<Func<T, TProperty>> navigationProperty,
        params Expression<Func<T, object>>[] includeExpressions)
        {
            // Comienza con la especificación principal
            IQueryable<T> query = _context.Set<T>().Where(specification.ToExpression());

            // Incluye las propiedades de navegación
            foreach (var includeExpression in includeExpressions)
            {
                query = query.Include(includeExpression);
            }

            // Construir la expresión combinada
            var navigationExpression = navigationSpecification.ToExpression();
            var parameter = Expression.Parameter(typeof(T), "entity");
            var navigationPropertyExpression = Expression.Invoke(navigationProperty, parameter);
            var combinedExpression = Expression.Invoke(navigationExpression, navigationPropertyExpression);

            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);

            // Aplicar la expresión combinada
            query = query.Where(lambda);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Obtiene todas las entidades del tipo especificado.
        /// </summary>
        /// <returns>Una colección de todas las entidades.</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Obtiene todas las entidades del tipo especificado, incluyendo múltiples propiedades de navegación.
        /// </summary>
        /// <param name="includes">Funciones para incluir propiedades de navegación y subpropiedades.</param>
        /// <returns>Una colección de todas las entidades.</returns>
        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Agrega una nueva entidad al contexto.
        /// </summary>
        /// <param name="entity">La entidad a agregar.</param>
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        /// <summary>
        /// Actualiza una entidad existente en el contexto.
        /// </summary>
        /// <param name="entity">La entidad a actualizar.</param>
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        /// <summary>
        /// Elimina una entidad del contexto.
        /// </summary>
        /// <param name="entity">La entidad a eliminar.</param>
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        /// <summary>
        /// Marca una entidad de usuario como eliminada, actualizando sus propiedades.
        /// </summary>
        /// <param name="entity">La entidad de usuario a marcar como eliminada.</param>
        public void MarkDeleted(T entity)
        {
            if (entity is User user)
            {
                string dateSuffix = "_deleted_" + DateTime.UtcNow.ToString("dd_MM_yyyy_HH_mm_ss_fff");

                user.DeletedAt = DateTime.UtcNow;
                user.UserName += dateSuffix;
                user.FirstName += dateSuffix;
                user.LastName += dateSuffix;
                user.Email += dateSuffix;
                user.NormalizedUserName += dateSuffix;
                user.NormalizedEmail += dateSuffix;

                _context.Set<User>().Update(user);

            }
            else if (entity is Review review)
            {
                review.DeletedAt = DateTime.UtcNow;
                _context.Set<Review>().Update(review);
            }

        }
    }
}
