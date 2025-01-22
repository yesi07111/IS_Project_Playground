using Playground.Application.Services;
using Playground.Domain.SmartEnum;

namespace Playground.Infraestructure.Services
{
    /// <summary>
    /// Implementación del servicio de imágenes para actividades.
    /// </summary>
    public class ImageService : IImageService
    {
        private static readonly HttpClient httpClient = new HttpClient();

        /// <inheritdoc />
        public string GetImageForActivity(Domain.Entities.Activity activity)
        {
            var facilityType = activity.Facility.Type;
            var facilityImageEnum = FacilityImageSmartEnum.FromName(facilityType);
            return facilityImageEnum.GetRandomImagePath();
        }

        /// <inheritdoc />
        public async Task<string> DownloadAndSaveUserProfileImageAsync(string imageUrl, string username)
        {
            var baseDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\"));
            var userImagesDir = Path.Combine(baseDir, "frontend", "public", "userImages");
            var userDir = Path.Combine(userImagesDir, username);

            if (!Directory.Exists(userDir))
            {
                Directory.CreateDirectory(userDir);
            }

            var localFileName = $"{Guid.NewGuid()}.jpg";
            var localFilePath = Path.Combine(userDir, localFileName);
            var relativePath = Path.Combine("userImages", username, localFileName);

            try
            {
                var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
                await File.WriteAllBytesAsync(localFilePath, imageBytes);
                return relativePath;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al descargar o guardar la imagen: {ex.Message}");
            }
        }
    }
}