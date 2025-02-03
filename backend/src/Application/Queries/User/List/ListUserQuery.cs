using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Queries.User.List;

/// <summary>
/// Consulta para listar usuarios del sistema con filtros avanzados.
/// Hereda de ICommand<ListUserResponse> para implementar el patrón de comando.
/// </summary>
public record ListUserQuery : ICommand<ListUserResponse>
{
    /// <summary>
    /// Filtro por nombre de usuario.
    /// </summary>
    public string? Username { get; init; }

    /// <summary>
    /// Filtro por dirección de correo electrónico.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Filtro por estado de confirmación del email ('true' o 'false').
    /// </summary>
    public string? EmailConfirmed { get; init; }

    /// <summary>
    /// Filtro por nombre del usuario.
    /// </summary>
    public string? FirstName { get; init; }

    /// <summary>
    /// Filtro por apellido del usuario.
    /// </summary>
    public string? LastName { get; init; }

    /// <summary>
    /// Filtro por rol asignado al usuario.
    /// </summary>
    public string? Rol { get; init; }

    /// <summary>
    /// Indica si incluir usuarios marcados como eliminados.
    /// </summary>
    public bool? MarkDeleted { get; init; }

    /// <summary>
    /// Caso de uso específico para la consulta de usuarios.
    /// </summary>
    public string UseCase { get; init; } = string.Empty;
}