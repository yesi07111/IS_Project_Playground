namespace Playground.Application.Queries.Dtos;

public class OnlyActivityDto
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
    /// Obtiene o establece el educador que gestiona la actividad.
    /// </summary>
    public string EducatorFirstName { get; set; } = string.Empty;
    public string EducatorLastName { get; set; } = string.Empty;

    public string EducatorUserName { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el tipo de actividad.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la edad recomendada para la actividad.
    /// </summary>
    public int RecommendedAge { get; set; }

    /// <summary>
    /// Indica si la actividad es privada.
    /// </summary>
    public bool ItsPrivate { get; set; }

    /// <summary>
    /// Obtiene o establece la instalación donde se lleva a cabo la actividad.
    /// </summary>
    public string FacilityName { get; set; } = string.Empty;
}