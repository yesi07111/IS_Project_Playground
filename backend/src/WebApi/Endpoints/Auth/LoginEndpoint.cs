using FastEndpoints;
using Playground.Application.Responses;
using Playground.Application.Queries.Auth.Login;

namespace Playground.WebApi.Endpoints;

/// <summary>
/// Endpoint para gestionar el proceso de inicio de sesión en el sistema.
/// </summary>
public class LoginEndpoint : Endpoint<LoginQuery, UserActionResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y define la ruta para el inicio de sesión.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/auth/login");
    }

    /// <summary>
    /// Maneja la solicitud para realizar el inicio de sesión y devuelve una respuesta con la acción del usuario.
    /// </summary>
    /// <param name="req">Consulta que contiene la información necesaria para realizar el inicio de sesión.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta con la acción que el sistema debe realizar después de intentar el inicio de sesión.</returns>
    public override Task<UserActionResponse> ExecuteAsync(LoginQuery req, CancellationToken ct)
    {
        return req.ExecuteAsync(ct);
    }
}
