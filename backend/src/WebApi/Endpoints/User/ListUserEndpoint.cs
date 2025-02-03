using FastEndpoints;
using Playground.Application.Queries.User.List;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.User;

/// <summary>
/// Endpoint para obtener una lista de usuarios.
/// </summary>
public class ListUserEndpoint : Endpoint<ListUserQuery, ListUserResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta de obtención de la lista de usuarios.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Get("/user/get-all");
    }

    /// <summary>
    /// Ejecuta la consulta para obtener la lista de usuarios procesando el query recibido.
    /// </summary>
    /// <param name="req">El query que contiene los criterios para listar los usuarios.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta con la lista de usuarios.</returns>
    public override async Task<ListUserResponse> ExecuteAsync(ListUserQuery req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
