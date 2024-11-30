namespace Playground.Domain.Entities.Common;

/// <summary>
/// Interfaz base para entidades que tienen un identificador único.
/// </summary>
/// <typeparam name="T">El tipo de la entidad que implementa la interfaz.</typeparam>
public interface IBaseEntity<T> where T : class
{
    /// <summary>
    /// Obtiene o establece el identificador único de la entidad.
    /// </summary>
    Guid Id { get; set; }
}