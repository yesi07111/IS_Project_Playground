namespace Playground.Application.Services;
public interface IConverterService
{
    /// <summary>
    /// Convierte una cadena de texto separada por comas en un IEnumerable de cadenas.
    /// </summary>
    /// <param name="input">Cadena de texto con elementos separados por comas.</param>
    /// <returns>IEnumerable de cadenas.</returns>
    IEnumerable<string> SplitStringToStringEnumerable(string input);

    /// <summary>
    /// Convierte una cadena de texto separada por comas en un IEnumerable de enteros.
    /// </summary>
    /// <param name="input">Cadena de texto con números separados por comas.</param>
    /// <returns>IEnumerable de enteros.</returns>
    IEnumerable<int> SplitStringToIntEnumerable(string input);

    /// <summary>
    /// Convierte un IEnumerable de enteros en un IEnumerable de DayOfWeek.
    /// </summary>
    /// <param name="days">IEnumerable de enteros representando días de la semana (0 = Domingo, ..., 6 = Sábado).</param>
    /// <returns>IEnumerable de DayOfWeek.</returns>
    IEnumerable<DayOfWeek> ConvertIntToDayOfWeek(IEnumerable<int> days);
}