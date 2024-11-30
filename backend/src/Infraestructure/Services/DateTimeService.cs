using Playground.Application.Services;

namespace Playground.Infraestructure.Services;

/// <summary>
/// Servicio para proporcionar fechas y horas actuales.
/// Ofrece tanto la hora local como la hora UTC.
/// </summary>
public class DateTimeService : IDateTimeService
{
    /// <summary>
    /// Obtiene la fecha y hora actual en la zona horaria local.
    /// </summary>
    public DateTime Now => DateTime.Now;

    /// <summary>
    /// Obtiene la fecha y hora actual en UTC.
    /// </summary>
    public DateTime UtcNow => DateTime.UtcNow;

    /// <summary>
    /// Obtiene la fecha actual sin la hora.
    /// </summary>
    public DateOnly Today => DateOnly.FromDateTime(Now);

    /// <summary>
    /// Obtiene la hora actual sin la fecha.
    /// </summary>
    public TimeOnly Time => TimeOnly.FromDateTime(Now);
}