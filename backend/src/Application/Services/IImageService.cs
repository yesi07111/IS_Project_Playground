namespace Playground.Application.Services
{
    /// <summary>
    /// Proporciona métodos para obtener imágenes relacionadas con actividades.
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Obtiene una imagen aleatoria para una actividad específica.
        /// </summary>
        /// <param name="activity">La actividad para la cual se desea obtener una imagen.</param>
        /// <returns>La ruta de la imagen aleatoria asociada a la actividad.</returns>
        string GetImageForActivity(Domain.Entities.Activity activity);

        /// <summary>
        /// Descarga y guarda una imagen de perfil de usuario.
        /// </summary>
        /// <param name="imageUrl">La URL de la imagen a descargar.</param>
        /// <param name="username">El nombre de usuario para el cual se guarda la imagen.</param>
        /// <returns>La ruta relativa donde se guardó la imagen.</returns>
        Task<string> DownloadAndSaveUserProfileImageAsync(string imageUrl, string username);
    }
}