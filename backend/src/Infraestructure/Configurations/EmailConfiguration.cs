namespace Playground.Infraestructure.Configurations;

/// <summary>
/// Configuración para el envío de correos electrónicos.
/// Contiene propiedades para configurar el servidor SMTP y las credenciales de correo.
/// </summary>
public class EmailConfiguration
{
    /// <summary>
    /// Dirección de correo electrónico del remitente.
    /// </summary>
    public string EmailFrom { get; set; } = string.Empty;

    /// <summary>
    /// Host del servidor SMTP.
    /// </summary>
    public string SmtpHost { get; set; } = string.Empty;

    /// <summary>
    /// Puerto del servidor SMTP.
    /// </summary>
    public int SmtpPort { get; set; }

    /// <summary>
    /// Usuario para autenticarse en el servidor SMTP.
    /// </summary>
    public string SmtpUser { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña para autenticarse en el servidor SMTP.
    /// </summary>
    public string SmtpPass { get; set; } = string.Empty;

    /// <summary>
    /// Nombre para mostrar en los correos electrónicos enviados.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico de soporte.
    /// </summary>
    public string SupportEmail { get; set; } = string.Empty;

    /// <summary>
    /// Indica si el modo de depuración está habilitado.
    /// </summary>
    public bool DebugMode { get; set; } = false;
}