using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Playground.Infraestructure.Configurations;

public sealed class JwtConfiguration
{
    public int LifetimeMinutes { get; set; } = 30;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string SecretKey { get; set; } = null!;

    public SymmetricSecurityKey EncryptedKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
}