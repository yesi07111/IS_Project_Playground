//newLaura

using Playground.Domain.Entities.Auth;
using Playground.Domain.Entities.Common;

namespace Playground.Domain.Entities
{
    /// <summary>
    /// Representa una reseña en el sistema.
    /// </summary>
    public class Review : IBaseEntity
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la reseña.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Obtiene o establece la fecha de la reseña.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Obtiene o establece el usuario padre que realizó la reseña.
        /// </summary>
        public User ParentId { get; set; }

        /// <summary>
        /// Obtiene o establece la instalación reseñada.
        /// </summary>
        public Facility Facility { get; set; }

        /// <summary>
        /// Obtiene o establece los comentarios de la reseña.
        /// </summary>
        public string Comments { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la puntuación de la reseña.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se creó la reseña.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se actualizó por última vez la reseña.
        /// </summary>
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se eliminó la reseña.
        /// </summary>
        public DateTime? DeletedAt { get; set; } = null;
    }
}