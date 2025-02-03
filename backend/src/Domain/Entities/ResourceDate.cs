using Playground.Domain.Entities.Common;

namespace Playground.Domain.Entities
{
    /// <summary>
    /// Representa un registro que asocia un recurso con una fecha específica, incluyendo la frecuencia de uso y las fechas de creación y actualización.
    /// </summary>
    public class ResourceDate : IBaseEntity
    {
        /// <summary>
        /// Obtiene o establece el identificador único del recurso.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Obtiene o establece el recurso asociado a esta fecha.
        /// </summary>
        public Resource Resource { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha asociada con el recurso.
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Obtiene o establece la frecuencia con la que se utiliza el recurso en esta fecha.
        /// </summary>
        public int UseFrequency { get; set; } 

        /// <summary>
        /// Obtiene o establece la fecha de creación del registro.
        /// </summary>
        public DateTime CreatedAt { get ; set; } = DateTime.UtcNow;

        /// <summary>
        /// Obtiene o establece la fecha de la última actualización del registro.
        /// </summary>
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Obtiene o establece la fecha de eliminación lógica del registro.
        /// </summary>
        public DateTime? DeletedAt { get ; set; } = null;
    }
}
