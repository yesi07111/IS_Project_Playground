using System.Linq.Expressions;
using Playground.Domain.Entities.Auth;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Application.Repositories
{
        /// <summary>
        /// Interfaz para un repositorio genérico que proporciona operaciones CRUD y consultas específicas.
        /// </summary>
        /// <typeparam name="T">El tipo de entidad gestionada por el repositorio.</typeparam>
        public interface IRepository<T>
        {
                /// <summary>
                /// Obtiene una entidad por su identificador de manera asincrónica.
                /// </summary>
                /// <param name="id">El identificador de la entidad, string.</param>
                /// <returns>Una tarea que representa la operación asincrónica, con la entidad encontrada o null si no existe.</returns>
                Task<T?> GetByIdAsync(string id);

                /// <summary>
                /// Obtiene una entidad por su identificador de manera asincrónica, incluyendo múltiples propiedades de navegación.
                /// </summary>
                /// <param name="id">El identificador de la entidad, string.</param>
                /// <param name="includes">Funciones para incluir propiedades de navegación y subpropiedades.</param>
                /// <returns>Una tarea que representa la operación asincrónica, con la entidad encontrada o null si no existe.</returns>
                Task<T?> GetByIdAsync(string id, params Expression<Func<T, object>>[] includes);

                /// <summary>
                /// Obtiene una entidad por su identificador de manera asincrónica.
                /// </summary>
                /// <param name="id">El identificador de la entidad, Guid.</param>
                /// <returns>Una tarea que representa la operación asincrónica, con la entidad encontrada o null si no existe.</returns>
                Task<T?> GetByIdAsync(Guid id);

                /// <summary>
                /// Obtiene una entidad por su identificador de manera asincrónica, incluyendo múltiples propiedades de navegación.
                /// </summary>
                /// <param name="id">El identificador de la entidad, Guid.</param>
                /// <param name="includes">Funciones para incluir propiedades de navegación y subpropiedades.</param>
                /// <returns>Una tarea que representa la operación asincrónica, con la entidad encontrada o null si no existe.</returns>
                Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes);

                /// <summary>
                /// Obtiene entidades que cumplen con una especificación dada de manera asincrónica.
                /// </summary>
                /// <param name="specification">La especificación que deben cumplir las entidades.</param>
                /// <returns>Una tarea que representa la operación asincrónica, con una colección de entidades que cumplen con la especificación.</returns>
                Task<IEnumerable<T>> GetBySpecificationAsync(ISpecification<T> specification);

                /// <summary>
                /// Obtiene entidades que cumplen con la especificación dada, incluyendo propiedades de navegación y subpropiedades.
                /// </summary>
                /// <param name="specification">La especificación que deben cumplir las entidades.</param>
                /// <param name="includeExpressions">Funciones para incluir propiedades de navegación y subpropiedades.</param>
                /// <returns>Una tarea que representa la operación asincrónica, con una colección de entidades que cumplen con la especificación.</returns>
                Task<IEnumerable<T>> GetBySpecificationAsync(ISpecification<T> specification, params Expression<Func<T, object>>[] includeExpressions);

                /// <summary>
                /// Obtiene entidades que cumplen con una especificación dada, y una especificación adicional para una propiedad de navegación.
                /// </summary>
                /// <typeparam name="TProperty">El tipo de la propiedad de navegación.</typeparam>
                /// <param name="specification">La especificación que deben cumplir las entidades.</param>
                /// <param name="navigationSpecification">La especificación que deben cumplir las entidades de la propiedad de navegación.</param>
                /// <param name="navigationProperty">Función para acceder a la propiedad de navegación.</param>
                /// <param name="includeExpressions">Funciones para incluir propiedades de navegación y subpropiedades.</param>
                /// <returns>Una tarea que representa la operación asincrónica, con una colección de entidades que cumplen con la especificación.</returns>
                Task<IEnumerable<T>> GetBySpecificationAsync<TProperty>(
                    ISpecification<T> specification,
                    ISpecification<TProperty> navigationSpecification,
                    Expression<Func<T, TProperty>> navigationProperty,
                    params Expression<Func<T, object>>[] includeExpressions);

                /// <summary>
                /// Obtiene todas las entidades del tipo especificado de manera asincrónica.
                /// </summary>
                /// <returns>Una tarea que representa la operación asincrónica, con una colección de todas las entidades.</returns>
                Task<IEnumerable<T>> GetAllAsync();

                /// <summary>
                /// Obtiene todas las entidades del tipo especificado de manera asincrónica, incluyendo múltiples propiedades de navegación.
                /// </summary>
                /// <param name="includes">Funciones para incluir propiedades de navegación y subpropiedades.</param>
                /// <returns>Una tarea que representa la operación asincrónica, con una colección de todas las entidades.</returns>
                Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);

                /// <summary>
                /// Agrega una nueva entidad al repositorio de manera asincrónica.
                /// </summary>
                /// <param name="entity">La entidad a agregar.</param>
                /// <returns>Una tarea que representa la operación asincrónica.</returns>
                Task AddAsync(T entity);

                /// <summary>
                /// Actualiza una entidad existente en el repositorio.
                /// </summary>
                /// <param name="entity">La entidad a actualizar.</param>
                void Update(T entity);

                /// <summary>
                /// Elimina una entidad del repositorio.
                /// </summary>
                /// <param name="entity">La entidad a eliminar.</param>
                void Delete(T entity);

                /// <summary>
                /// Marca una entidad de usuario como eliminada, actualizando sus propiedades.
                /// </summary>
                /// <param name="entity">La entidad de usuario a marcar como eliminada.</param>
                void MarkDeleted(T entity);
        }
}