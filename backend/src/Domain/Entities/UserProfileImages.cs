using Playground.Domain.Entities.Auth;
using Playground.Domain.Entities.Common;

namespace Playground.Domain.Entities;

/// <summary>
/// Representa una actividad en el sistema.
/// </summary>
public class UserProfileImages : IBaseEntity
{
    /// <summary>
    /// Obtiene o establece el identificador único de la actividad.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Obtiene o establece el path a la imagen de perfil.
    /// </summary>
    public string ProfileImage { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el path a otras imágenes subidas por el usuario.
    /// </summary>
    public string OtherImages { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el usuario al que se asocia la imagen.
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Obtiene o establece la fecha y hora en que se creó la actividad.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Obtiene o establece la fecha y hora en que se actualizó por última vez la actividad.
    /// </summary>
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Obtiene o establece la fecha y hora en que se eliminó la actividad.
    /// </summary>
    public DateTime? DeletedAt { get; set; } = null;

}