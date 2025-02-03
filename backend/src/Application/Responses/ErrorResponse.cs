namespace Playground.Application.Responses
{
    /// <summary>
    /// Representa una respuesta de error genérica.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Obtiene o establece el tipo de error.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Obtiene o establece el título del error.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Obtiene o establece los detalles del error.
        /// </summary>
        public string? Detail { get; set; }

        /// <summary>
        /// Obtiene o establece el código de estado HTTP asociado con el error.
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Obtiene o establece un código específico del cliente para el error.
        /// </summary>
        public int? ClientCode { get; set; }

        /// <summary>
        /// Obtiene o establece una instancia específica de la respuesta de error.
        /// </summary>
        public string? Instance { get; set; }
    }
}
