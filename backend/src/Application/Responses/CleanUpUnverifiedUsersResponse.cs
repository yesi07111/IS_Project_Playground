namespace Playground.Application.Responses;

/// <summary>
/// Representa la respuesta de la operación de limpieza de usuarios no verificados.
/// </summary>
public record CleanUpUnverifiedUsersResponse(int DeletedUsersCount);