namespace Playground.Domain.Entities.Common;

/// <summary>
/// Interfaz base para entidades que tienen un identificador único.
/// </summary>
public interface IBaseEntity
{
    /// <summary>
    /// Obtiene o establece el identificador único de la entidad.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Obtiene o establece la fecha y hora en que se creó la entidad.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Obtiene o establece la fecha y hora en que se actualizó por última vez la entidad.
    /// </summary>
    public DateTime UpdateAt { get; set; }

    /// <summary>
    /// Obtiene o establece la fecha y hora en que se eliminó la entidad.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}