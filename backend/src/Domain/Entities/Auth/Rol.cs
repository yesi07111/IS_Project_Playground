namespace Playground.Domain.Entities.Auth
{
    /// <summary>
    /// Representa un rol de usuario en el sistema, incluyendo el nombre del rol y la colección de usuarios asociados a este rol.
    /// </summary>
    public class Rol
    {
        /// <summary>
        /// Obtiene o establece el identificador único del rol.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Obtiene o establece el nombre del rol.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la colección de usuarios que tienen este rol.
        /// </summary>
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
