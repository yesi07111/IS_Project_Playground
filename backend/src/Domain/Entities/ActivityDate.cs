using Playground.Domain.Entities.Auth;
using Playground.Domain.Entities.Common;

namespace Playground.Domain.Entities
{
    /// <summary>
    /// Representa una actividad en el sistema.
    /// </summary>
    public class ActivityDate : IBaseEntity
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la actividad en una fecha.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Obtiene o establece la actividad asociada a la fecha.
        /// </summary>
        public Activity Activity { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de la actividad.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Obtiene o establece el número actual de participantes en la actividad.
        /// </summary>
        public int ReservedPlaces { get; set; } = 0;

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se creó la actividad en una fecha.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se actualizó por última vez la actividad en una fecha.
        /// </summary>
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se eliminó la actividad.
        /// </summary>
        public DateTime? DeletedAt { get; set; } = null;


        /// <summary>
        /// Agrega un participante a la actividad si no se ha alcanzado la capacidad máxima.
        /// </summary>
        public void AddParticipant()
        {
            if (ReservedPlaces < Activity.Facility.MaximumCapacity)
            {
                ReservedPlaces++;
            }
            else
            {
                throw new InvalidOperationException("No se pueden agregar más participantes a esta actividad.");
            }
        }
    }
}