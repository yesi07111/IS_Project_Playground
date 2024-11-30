namespace Playground.Application.Services
{
    /// <summary>
    /// Interfaz para un servicio de limpieza que gestiona el inicio y la detención de procesos de limpieza.
    /// </summary>
    public interface ICleanUpService
    {
        /// <summary>
        /// Inicia el servicio de limpieza de manera asincrónica.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para la operación.</param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        Task StartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Detiene el servicio de limpieza de manera asincrónica.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para la operación.</param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        Task StopAsync(CancellationToken cancellationToken);
    }
}