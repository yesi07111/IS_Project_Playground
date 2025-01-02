using Playground.Application.Services;

namespace Playground.Infraestructure.Services;
public class ConverterService : IConverterService
{
    /// <summary>
    /// Convierte una cadena de texto separada por comas en un IEnumerable de cadenas.
    /// </summary>
    /// <param name="input">Cadena de texto con elementos separados por comas.</param>
    /// <returns>IEnumerable de cadenas.</returns>
    public IEnumerable<string> SplitStringToStringEnumerable(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return Enumerable.Empty<string>();
        }

        return input.Split(',')
                    .Select(item => item.Trim())
                    .Where(item => !string.IsNullOrEmpty(item));
    }

    /// <summary>
    /// Convierte una cadena de texto separada por comas en un IEnumerable de enteros.
    /// </summary>
    /// <param name="input">Cadena de texto con números separados por comas.</param>
    /// <returns>IEnumerable de enteros.</returns>
    public IEnumerable<int> SplitStringToIntEnumerable(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return Enumerable.Empty<int>();
        }

        return input.Split(',')
                    .Select(item => item.Trim())
                    .Where(item => !string.IsNullOrEmpty(item))
                    .Select(item => int.TryParse(item, out int number) ? number : (int?)null)
                    .Where(number => number.HasValue)
                    .Select(number => number!.Value);
    }

    /// <summary>
    /// Convierte un IEnumerable de enteros en un IEnumerable de DayOfWeek.
    /// </summary>
    /// <param name="days">IEnumerable de enteros representando días de la semana (0 = Domingo, ..., 6 = Sábado).</param>
    /// <returns>IEnumerable de DayOfWeek.</returns>
    public IEnumerable<DayOfWeek> ConvertIntToDayOfWeek(IEnumerable<int> days)
    {
        return days.Select(day => (DayOfWeek)day);
    }
}