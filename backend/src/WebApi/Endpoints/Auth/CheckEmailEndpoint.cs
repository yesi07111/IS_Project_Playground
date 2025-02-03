using FastEndpoints;
using Playground.Application.Queries.CheckEmail;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints;

/// <summary>
/// Endpoint para verificar si un correo electrónico ya está registrado.
/// </summary>
public class CheckEmailEndpoint : Endpoint<CheckEmailQuery, CheckEmailResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta para verificar el correo electrónico.
    /// </summary>
    public override void Configure()
    {
        Get("auth/check-email");
        AllowAnonymous();
    }

    /// <summary>
    /// Ejecuta la verificación del correo electrónico proporcionado en la consulta.
    /// </summary>
    /// <param name="req">El comando que contiene el correo electrónico a verificar.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta que indica si el correo electrónico está disponible o ya está registrado.</returns>
    public override Task<CheckEmailResponse> ExecuteAsync(CheckEmailQuery req, CancellationToken ct)
    {
        return req.ExecuteAsync(ct);
    }
}
