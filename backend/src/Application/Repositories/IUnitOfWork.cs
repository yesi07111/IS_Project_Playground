namespace Playground.Application.Repositories;

/// <summary>
/// Interfaz para una unidad de trabajo que gestiona las transacciones en el contexto de base de datos.
/// Proporciona métodos para confirmar cambios en el contexto.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Confirma de forma asincrónica todos los cambios realizados en el contexto de base de datos.
    /// </summary>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    Task CommitAsync();

    /// <summary>
    /// Confirma todos los cambios realizados en el contexto de base de datos.
    /// </summary>
    void Commit();
}