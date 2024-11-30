using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Playground.Infraestructure.Configurations;

/// <summary>
/// Configuración para la generación de tokens JWT.
/// Contiene propiedades para configurar el emisor, audiencia, clave secreta y duración del token.
/// </summary>
public sealed class JwtConfiguration
{
    /// <summary>
    /// Duración del token en minutos.
    /// </summary>
    public int LifetimeMinutes { get; set; } = 30;

    /// <summary>
    /// Emisor del token.
    /// </summary>
    public string Issuer { get; set; } = null!;

    /// <summary>
    /// Audiencia del token.
    /// </summary>
    public string Audience { get; set; } = null!;

    /// <summary>
    /// Clave secreta utilizada para firmar el token.
    /// </summary>
    public string SecretKey { get; set; } = null!;

    /// <summary>
    /// Obtiene la clave de seguridad simétrica generada a partir de la clave secreta.
    /// </summary>
    public SymmetricSecurityKey EncryptedKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
}