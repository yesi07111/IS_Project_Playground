namespace Playground.Application.Services;

/// <summary>
/// Interfaz para un servicio de fecha y hora que proporciona acceso a la fecha y hora actuales.
/// </summary>
public interface IDateTimeService
{
    /// <summary>
    /// Obtiene la fecha y hora actuales.
    /// </summary>
    DateTime Now { get; }

    /// <summary>
    /// Obtiene la fecha y hora actuales en UTC.
    /// </summary>
    DateTime UtcNow { get; }

    /// <summary>
    /// Obtiene la fecha actual.
    /// </summary>
    DateOnly Today { get; }

    /// <summary>
    /// Obtiene la hora actual.
    /// </summary>
    TimeOnly Time { get; }
}