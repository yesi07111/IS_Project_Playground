namespace Playground.Application.Responses;

/// <summary>
/// Representa la respuesta para obtener la información de la página de inicio.
/// </summary>
public record GetHomePageInfoResponse(int Visitors, int ActiveActivities, double Score);