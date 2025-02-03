using FastEndpoints;
using Playground.Application.Commands.CleanUp;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints;

/// <summary>
/// Endpoint para limpiar usuarios no verificados.
/// </summary>
public class CleanUpUnverifiedUsersEndpoint : EndpointWithoutRequest<CleanUpUnverifiedUsersResponse>
{
    private readonly CleanUpUnverifiedUsersCommandHandler _commandHandler;

    /// <summary>
    /// Inicializa el endpoint con el manejador del comando para limpiar usuarios no verificados.
    /// </summary>
    /// <param name="commandHandler">El manejador del comando para realizar la limpieza de usuarios no verificados.</param>
    public CleanUpUnverifiedUsersEndpoint(CleanUpUnverifiedUsersCommandHandler commandHandler)
    {
        _commandHandler = commandHandler;
    }

    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y definir la ruta para limpiar usuarios no verificados.
    /// </summary>
    public override void Configure()
    {
        Delete("auth/cleanup-unverified-users");
        AllowAnonymous();
    }

    /// <summary>
    /// Ejecuta la limpieza de usuarios no verificados.
    /// </summary>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta indicando el resultado de la operación de limpieza.</returns>
    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await _commandHandler.ExecuteAsync(ct);
        await SendAsync(response, cancellation: ct);
    }
}
