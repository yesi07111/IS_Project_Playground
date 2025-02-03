namespace Playground.Application.Dtos
{
    /// <summary>
    /// Representa los datos de una reseña realizada por un usuario sobre una actividad.
    /// </summary>
    public class ReviewDto
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la reseña.
        /// </summary>
        public string ReviewId { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el identificador de la actividad sobre la que se realiza la reseña.
        /// </summary>
        public string ActivityId { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el nombre de la actividad sobre la que se realiza la reseña.
        /// </summary>
        public string ActivityName { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el comentario escrito por el usuario sobre la actividad.
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la calificación dada por el usuario a la actividad.
        /// La calificación está en una escala de 1 a 5.
        /// </summary>
        public int Rating { get; set; }
    }
}
