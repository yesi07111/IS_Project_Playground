//newLaura

using Playground.Domain.Entities.Common;

namespace Playground.Domain.Entities
{
    /// <summary>
    /// Representa una instalación en el sistema.
    /// </summary>
    public class Facility : IBaseEntity
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la instalación.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Obtiene o establece el nombre de la instalación.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la ubicación de la instalación.
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el tipo de instalación.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la capacidad máxima de la instalación.
        /// </summary>
        public int MaximumCapacity { get; set; }

        /// <summary>
        /// Obtiene o establece la política de uso de la instalación.
        /// </summary>
        public string UsagePolicy { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se creó la instalación.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se actualizó por última vez la instalación.
        /// </summary>
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se eliminó la instalación.
        /// </summary>
        public DateTime? DeletedAt { get; set; } = null;
    }
}