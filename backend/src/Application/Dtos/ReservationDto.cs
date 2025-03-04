using System.Data.SqlTypes;

namespace Playground.Application.Dtos
{
    /// <summary>
    /// Representa los datos de una reserva realizada por un usuario para una actividad.
    /// </summary>
    public class ReservationDto
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la reserva.
        /// </summary>
        public string ReservationId { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el primer nombre del usuario que realizó la reserva.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el apellido del usuario que realizó la reserva.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el nombre de usuario del usuario que realizó la reserva.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el identificador de la actividad para la que se realiza la reserva.
        /// </summary>
        public string ActivityId { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el nombre de la actividad para la que se realiza la reserva.
        /// </summary>
        public string ActivityName { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la fecha de la actividad para la que se realiza la reserva.
        /// </summary>
        public string ActivityDate { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la cantidad de personas para la reserva.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Obtiene o establece los comentarios adicionales sobre la reserva.
        /// </summary>
        public string Comments { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el estado de la reserva (por ejemplo, confirmada, pendiente).
        /// </summary>
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la edad recomendada para la actividad para la que se realiza la reserva.
        /// </summary>
        public int ActivityRecommendedAge { get; set; }

        public int UsedCapacity { get; set; }
        public int Capacity { get; set; }
    }
}
