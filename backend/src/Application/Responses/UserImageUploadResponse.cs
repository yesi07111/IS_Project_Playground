namespace Playground.Application.Responses;

/// <summary>
/// Representa la respuesta al proceso de carga de imágenes del usuario, incluyendo la URL de la imagen principal y otras referencias relacionadas.
/// </summary>
public record UserImageUploadResponse(string ImageUrl, IEnumerable<string> Others);