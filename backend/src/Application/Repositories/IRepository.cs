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
                /// <param name="id">El identificador de la entidad.</param>
                /// <returns>Una tarea que representa la operación asincrónica, con la entidad encontrada o null si no existe.</returns>
                Task<T?> GetByIdAsync(string id);

                /// <summary>
                /// Obtiene entidades que cumplen con una especificación dada de manera asincrónica.
                /// </summary>
                /// <param name="specification">La especificación que deben cumplir las entidades.</param>
                /// <returns>Una tarea que representa la operación asincrónica, con una colección de entidades que cumplen con la especificación.</returns>
                Task<IEnumerable<T>> GetBySpecificationAsync(ISpecification<T> specification);

                /// <summary>
                /// Obtiene todas las entidades del tipo especificado de manera asincrónica.
                /// </summary>
                /// <returns>Una tarea que representa la operación asincrónica, con una colección de todas las entidades.</returns>
                Task<IEnumerable<T>> GetAllAsync();

                /// <summary>
                /// Obtiene todos los usuarios con el rol de administrador de manera asincrónica.
                /// </summary>
                /// <returns>Una tarea que representa la operación asincrónica, con una colección de usuarios administradores.</returns>
                Task<IEnumerable<User>> GetAllAdminsAsync();

                /// <summary>
                /// Obtiene todos los usuarios con el rol de educador de manera asincrónica.
                /// </summary>
                /// <returns>Una tarea que representa la operación asincrónica, con una colección de usuarios educadores.</returns>
                Task<IEnumerable<User>> GetAllEducatorsAsync();

                /// <summary>
                /// Obtiene todos los usuarios con el rol de padre de manera asincrónica.
                /// </summary>
                /// <returns>Una tarea que representa la operación asincrónica, con una colección de usuarios padres.</returns>
                Task<IEnumerable<User>> GetAllParentsAsync();

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
                void MarkDeleted(User entity);
        }
}