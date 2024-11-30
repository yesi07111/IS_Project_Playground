namespace Playground.Domain.Events
{
    /// <summary>
    /// Evento que representa una solicitud de rol por parte de un usuario.
    /// Contiene información sobre el usuario que solicita el rol y el rol solicitado.
    /// </summary>
    public class RoleRequestedEvent
    {
        /// <summary>
        /// Obtiene el identificador del usuario que solicita el rol.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Obtiene el rol solicitado por el usuario.
        /// </summary>
        public string Role { get; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RoleRequestedEvent"/>.
        /// </summary>
        /// <param name="userId">El identificador del usuario que solicita el rol.</param>
        /// <param name="role">El rol solicitado.</param>
        public RoleRequestedEvent(string userId, string role)
        {
            UserId = userId;
            Role = role;
        }
    }
}