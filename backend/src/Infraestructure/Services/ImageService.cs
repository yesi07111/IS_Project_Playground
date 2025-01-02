using Playground.Application.Services;
using Playground.Domain.SmartEnum;

namespace Playground.Infraestructure.Services
{
    /// <summary>
    /// Implementación del servicio de imágenes para actividades.
    /// </summary>
    public class ImageService : IImageService
    {
        /// <inheritdoc />
        public string GetImageForActivity(Domain.Entities.Activity activity)
        {
            var facilityType = activity.Facility.Type;
            var facilityImageEnum = FacilityImageSmartEnum.FromName(facilityType);
            return facilityImageEnum.GetRandomImagePath();
        }
    }
}