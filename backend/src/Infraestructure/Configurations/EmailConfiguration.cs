namespace Playground.Infraestructure.Configurations;

public class EmailConfiguration
{
    public string EmailFrom { get; set; } = string.Empty;
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
    public string SmtpUser { get; set; } = string.Empty;
    public string SmtpPass { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string SupportEmail { get; set; } = string.Empty;
    public bool DebugMode { get; set; } = false;
}