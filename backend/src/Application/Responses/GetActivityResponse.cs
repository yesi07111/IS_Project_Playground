using Playground.Application.Dtos;

namespace Playground.Application.Responses;

/// <summary>
/// Representa la respuesta para obtener los detalles de una actividad.
/// </summary>
public record GetActivityResponse(ActivityDetailDto Result);