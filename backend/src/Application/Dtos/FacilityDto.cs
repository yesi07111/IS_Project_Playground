namespace Playground.Application.Dtos
{
    /// <summary>
    /// DTO que representa una actividad para el frontend.
    /// </summary>
    public class FacilityDto
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la instalación.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el nombre de la instalación.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la ubicación de la instalación.
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el tipo de la instalación.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la política de uso de la instalación.
        /// </summary>
        public string UsagePolicy { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la capacidad máxima de la instalación.
        /// </summary>
        public int MaximumCapacity { get; set; }
    }
}