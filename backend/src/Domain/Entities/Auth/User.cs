using Microsoft.AspNetCore.Identity;

namespace Playground.Domain.Entities.Auth;

/// <summary>
/// Representa un usuario en el sistema, extendiendo la funcionalidad de <see cref="IdentityUser"/>.
/// Incluye propiedades adicionales para gestionar información del usuario.
/// </summary>
/// <summary>
/// Representa un usuario en el sistema, extendiendo la funcionalidad de <see cref="IdentityUser"/>.
/// Incluye propiedades adicionales para gestionar información del usuario.
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// Obtiene o establece el primer nombre del usuario.
    /// </summary>
    /// <summary>
    /// Obtiene o establece el primer nombre del usuario.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el apellido del usuario.
    /// </summary>

    /// <summary>
    /// Obtiene o establece el apellido del usuario.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el código completo del usuario.
    /// </summary>

    /// <summary>
    /// Obtiene o establece el código completo del usuario.
    /// </summary>
    public string FullCode { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la fecha de creación del usuario.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Obtiene o establece la fecha de última actualización del usuario.
    /// </summary>
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Obtiene o establece la fecha de eliminación del usuario.
    /// </summary>
    public DateTime? DeletedAt { get; set; } = null;
}