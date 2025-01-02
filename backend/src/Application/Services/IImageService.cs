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
    }
}