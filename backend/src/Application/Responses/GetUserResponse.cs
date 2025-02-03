using Playground.Application.Dtos;

namespace Playground.Application.Responses;

/// <summary>
/// Representa la respuesta para obtener la información de un usuario.
/// </summary>
public record GetUserResponse(UserDto Result);