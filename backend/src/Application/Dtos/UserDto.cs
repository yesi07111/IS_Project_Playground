namespace Playground.Application.Dtos
{
    /// <summary>
    /// DTO que representa una actividad para el frontend.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Obtiene o establece el identificador único del usuario.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el nombre de usuario.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el nombre real del usuario.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el apellido de usuario.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el correo electrónico del usuario.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el rol del usuario.
        /// </summary>
        public string Rol { get; set; } = string.Empty;
    }
}