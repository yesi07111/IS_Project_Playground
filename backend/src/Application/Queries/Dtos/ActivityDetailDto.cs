namespace Playground.Application.Queries.Dtos
{
    /// <summary>
    /// DTO que representa una actividad para el frontend.
    /// </summary>
    public class ActivityDetailDto
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
        /// Obtiene o establece la descripción de la actividad.
        /// </summary>
        public string Description { get; set; } = string.Empty;

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
        /// Obtiene o establece el nombre completo del educador.
        /// </summary>
        public string EducatorFullName { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el nombre de usuario del educador.
        /// </summary>
        public string EducatorUsername { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el nombre de la instalación.
        /// </summary>
        public string FacilityName { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la ubicación de la instalación.
        /// </summary>
        public string FacilityLocation { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el tipo de la instalación.
        /// </summary>
        public string FacilityType { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el tipo de la actividad.
        /// </summary>
        public string ActivityType { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la política de uso de la actividad.
        /// </summary>
        public string UsagePolicy { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la edad recomendada para la actividad.
        /// </summary>
        public string RecommendedAge { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece los comentarios sobre la actividad.
        /// </summary>
        public IEnumerable<string> Comments { get; set; } = [];

        /// <summary>
        /// Obtiene o establece los recursos disponibles para la actividad.
        /// </summary>
        public IEnumerable<string> Resources { get; set; } = [];

        /// <summary>
        /// Obtiene o establece la fecha de la actividad.
        /// </summary>
        public DateTime Date { get; set; }
    }
}