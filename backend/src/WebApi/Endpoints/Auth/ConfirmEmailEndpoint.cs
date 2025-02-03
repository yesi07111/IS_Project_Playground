using Playground.Application.Queries.Auth.ConfirmEmail;
using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Auth;

/// <summary>
/// Endpoint para confirmar el correo electrónico de un usuario.
/// </summary>
public class ConfirmEmailEndpoint : Endpoint<ConfirmEmailQuery, UserActionResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta para confirmar el correo electrónico.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/auth/confirm-email");
    }

    /// <summary>
    /// Ejecuta la confirmación del correo electrónico del usuario.
    /// </summary>
    /// <param name="req">El objeto de consulta que contiene los datos necesarios para la confirmación del correo electrónico.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta indicando el resultado de la operación de confirmación de correo electrónico.</returns>
    public override async Task<UserActionResponse> ExecuteAsync(ConfirmEmailQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
