namespace Playground.Application.Dtos
{
    /// <summary>
    /// DTO que representa una actividad para el frontend.
    /// </summary>
    public class ActivityDto
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la actividad.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la actividad.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la fecha de la actividad.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Obtiene o establece la URL de la imagen representativa de la actividad.
        /// </summary>
        public string Image { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la calificación de la actividad.
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// Obtiene o establece el color asociado a la actividad para temas de diseño.
        /// </summary>
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la capacidad máxima asociada a la actividad.
        /// </summary>
        public int MaximumCapacity { get; set; }

        /// <summary>
        /// Obtiene o establece la capacidad actual ocupada asociado a la actividad.
        /// </summary>
        public int CurrentCapacity { get; set; }

        /// <summary>
        /// Obtiene o establece si la actividad es pública o no.
        /// </summary>
        public string IsPublic { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece si la actividad es nueva o no.
        /// </summary>
        public string IsNew { get; set; } = string.Empty;
    }
}