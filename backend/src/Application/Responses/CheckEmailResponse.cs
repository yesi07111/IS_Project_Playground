namespace Playground.Application.Responses;

/// <summary>
/// Representa la respuesta para verificar el estado de verificación de un correo electrónico.
/// </summary>
public record CheckEmailResponse(bool IsVerified);