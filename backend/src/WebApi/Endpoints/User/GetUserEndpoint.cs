using FastEndpoints;
using Playground.Application.Queries.User.Get;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.User;

/// <summary>
/// Endpoint para obtener información de un usuario.
/// </summary>
public class GetUserEndpoint : Endpoint<GetUserQuery, GetUserResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de obtención de usuario.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/user/get");
    }

    /// <summary>
    /// Ejecuta la consulta para obtener la información del usuario procesando el query recibido.
    /// </summary>
    /// <param name="req">El query que contiene los criterios para obtener la información del usuario.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta con la información del usuario.</returns>
    public override async Task<GetUserResponse> ExecuteAsync(GetUserQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
