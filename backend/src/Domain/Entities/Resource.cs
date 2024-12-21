//newLaura

using Playground.Domain.Entities.Common;

namespace Playground.Domain.Entities
{
    /// <summary>
    /// Representa un recurso en el sistema.
    /// </summary>
    public class Resource : IBaseEntity
    {
        /// <summary>
        /// Obtiene o establece el identificador único del recurso.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Obtiene o establece el nombre del recurso.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el tipo de recurso.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la frecuencia de uso del recurso.
        /// </summary>
        public float UseFrecuency { get; set; }

        /// <summary>
        /// Obtiene o establece la condición del recurso.
        /// </summary>
        public string ResourceCondition { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la instalación asociada al recurso.
        /// </summary>
        public Facility Facility { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se creó el recurso.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se actualizó por última vez el recurso.
        /// </summary>
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se eliminó el recurso.
        /// </summary>
        public DateTime? DeletedAt { get; set; } = null;
    }
}