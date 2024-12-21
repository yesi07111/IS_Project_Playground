using Playground.Domain.Entities.Common;

namespace Playground.Domain.Entities
{
    /// <summary>
    /// Representa una notificación en el sistema.
    /// Implementa la interfaz <see cref="IBaseEntity{Notification}"/> para proporcionar un identificador único.
    /// </summary>
    public class Notification : IBaseEntity
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la notificación.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Obtiene o establece el remitente de la notificación.
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha en que se envió la notificación.
        /// </summary>
        public DateTime SentDate { get; set; }

        /// <summary>
        /// Obtiene o establece el mensaje de la notificación.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se creó la notificación.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se actualizó por última vez la notificación.
        /// </summary>
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se eliminó la notificación.
        /// </summary>
        public DateTime? DeletedAt { get; set; } = null;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Notification"/>.
        /// </summary>
        /// <param name="sender">El remitente de la notificación.</param>
        /// <param name="message">El mensaje de la notificación.</param>
        public Notification(string sender, string message)
        {
            Sender = sender;
            SentDate = DateTime.UtcNow;
            Message = message;
        }
    }
}