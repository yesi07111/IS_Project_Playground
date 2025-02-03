using FastEndpoints;
using Playground.Application.Commands.Auth.UploadUserImage;
using Playground.Application.Responses;

namespace Playground.WebApi.Endpoints.Auth;

/// <summary>
/// Endpoint para cargar una imagen de usuario.
/// </summary>
public class UploadUserImageEndpoint : Endpoint<UploadUserImageCommand, UserImageUploadResponse>
{
    /// <summary>
    /// Configura el endpoint para permitir la carga de archivos y define la ruta para cargar una imagen de usuario.
    /// </summary>
    public override void Configure()
    {
        Post("/auth/upload-user-image");
        AllowAnonymous();
        AllowFileUploads();
    }

    /// <summary>
    /// Maneja la solicitud para cargar la imagen de usuario.
    /// </summary>
    /// <param name="req">Comando que contiene la imagen de usuario a cargar.</param>
    /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
    /// <returns>Una respuesta que indica el resultado de la carga de la imagen.</returns>
    public override async Task<UserImageUploadResponse> ExecuteAsync(UploadUserImageCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}
