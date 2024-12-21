using Playground.Domain.Entities.Auth;
using Playground.Domain.Entities.Common;

namespace Playground.Domain.Entities
{
    /// <summary>
    /// Representa una actividad en el sistema.
    /// </summary>
    public class Activity : IBaseEntity
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la actividad.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Obtiene o establece el nombre de la actividad.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la fecha de la actividad.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción de la actividad.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el número actual de participantes en la actividad.
        /// </summary>

        /// <summary>
        /// Obtiene o establece el número actual de participantes en la actividad.
        /// </summary>
        public int CurrentParticipants { get; set; } = 0;

        /// <summary>
        /// Obtiene o establece el educador que gestiona la actividad.
        /// </summary>
        public User Educator { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de actividad.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la edad recomendada para la actividad.
        /// </summary>

        /// <summary>
        /// Obtiene o establece la edad recomendada para la actividad.
        /// </summary>
        public int RecommendedAge { get; set; }

        /// <summary>
        /// Indica si la actividad es privada.
        /// </summary>

        /// <summary>
        /// Indica si la actividad es privada.
        /// </summary>
        public bool ItsPrivate { get; set; }

        /// <summary>
        /// Obtiene o establece la instalación donde se lleva a cabo la actividad.
        /// </summary>
        public Facility Facility { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se creó la actividad.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se actualizó por última vez la actividad.
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
            if (CurrentParticipants < Facility.MaximumCapacity)
            {
                CurrentParticipants++;
            }
            else
            {
                throw new InvalidOperationException("No se pueden agregar más participantes a esta actividad.");
            }
        }
    }
}