using Playground.Application.Repositories;

namespace Playground.Application.Factories;

/// <summary>
/// Interfaz para una fábrica de repositorios que proporciona un método para crear instancias de repositorios genéricos.
/// </summary>
public interface IRepositoryFactory
{
    /// <summary>
    /// Crea un repositorio para un tipo de entidad específico.
    /// </summary>
    /// <typeparam name="T">El tipo de entidad para el cual se crea el repositorio.</typeparam>
    /// <returns>Una instancia de <see cref="IRepository{T}"/> para el tipo de entidad especificado.</returns>
    IRepository<T> CreateRepository<T>() where T : class;
}