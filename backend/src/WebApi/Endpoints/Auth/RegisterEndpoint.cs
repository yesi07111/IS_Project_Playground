using Playground.Application.Commands.Auth.Register;
using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Auth;

/// <summary>
/// Endpoint para gestionar el proceso de registro de un nuevo usuario.
/// </summary>
public class RegisterEndpoint : Endpoint<RegisterCommand, UserCreationResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir solicitudes anónimas y define la ruta para el registro de un nuevo usuario.
    /// </summary>
    public override void Configure()
    {
        AllowAnonymous();
        Post("/auth/register");
    }

    /// <summary>
    /// Maneja la solicitud para registrar un nuevo usuario y devuelve una respuesta con los detalles de la creación del usuario.
    /// </summary>
    /// <param name="req">Comando que contiene la información necesaria para registrar al nuevo usuario.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta con los detalles del usuario creado, como su ID y estado de la creación.</returns>
    public override async Task<UserCreationResponse> ExecuteAsync(RegisterCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
