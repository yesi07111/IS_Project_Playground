//newLaura

using Playground.Domain.Entities.Auth;
using Playground.Domain.Entities.Common;

namespace Playground.Domain.Entities
{
    /// <summary>
    /// Representa una reserva en el sistema.
    /// </summary>
    public class Reservation : IBaseEntity
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la reserva.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Obtiene o establece el usuario padre asociado a la reserva.
        /// </summary>
        public User Parent { get; set; }

        /// <summary>
        /// Obtiene o establece la actividad y fecha reservada.
        /// </summary>
        public ActivityDate ActivityDate { get; set; }

        /// <summary>
        /// Obtiene o establece comentarios adicionales para la reserva.
        /// </summary>
        public string AdditionalComments { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la cantidad de niños en la reserva.
        /// </summary>
        public int AmmountOfChildren { get; set; }

        /// <summary>
        /// Obtiene o establece el estado de la reserva.
        /// </summary>
        public string ReservationState { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se creó la reserva.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se actualizó por última vez la reserva.
        /// </summary>
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se eliminó la reserva.
        /// </summary>
        public DateTime? DeletedAt { get; set; } = null;
    }
}