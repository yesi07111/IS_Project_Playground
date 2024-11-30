using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Playground.Application.Services;
using Playground.Domain.Entities.Auth;
using Playground.Infraestructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Playground.Infraestructure.Services;

/// <summary>
/// Generador de tokens JWT para la autenticación de usuarios.
/// Utiliza configuraciones de JWT y servicios de identidad para crear tokens.
/// </summary>
public class JwtGenerator : IJwtGenerator
{
    private readonly JwtConfiguration options;
    private readonly UserManager<User> userManager;
    private readonly IDateTimeService dateTimeService;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="JwtGenerator"/>.
    /// </summary>
    /// <param name="_options">Configuración de JWT.</param>
    /// <param name="_userManager">Administrador de usuarios para gestionar la identidad de usuario.</param>
    /// <param name="_dateTimeService">Servicio para obtener la fecha y hora actuales.</param>
    public JwtGenerator(IOptions<JwtConfiguration> _options, UserManager<User> _userManager, IDateTimeService _dateTimeService)
    {
        options = _options.Value;
        userManager = _userManager;
        dateTimeService = _dateTimeService;
    }

    /// <summary>
    /// Genera un token JWT para un usuario específico.
    /// </summary>
    /// <param name="user">El usuario para el que se genera el token.</param>
    /// <returns>Una tarea que representa la operación asincrónica, con el token JWT como resultado.</returns>
    public async Task<string> GetToken(User user)
    {
        var credentials = new SigningCredentials(options.EncryptedKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.NameIdentifier, user.Id ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
        };

        var roles = (await userManager.GetRolesAsync(user))
                                      .Select(r => new Claim(ClaimTypes.Role, r));

        claims.AddRange(roles);

        var token = new JwtSecurityToken(options.Issuer,
                                         options.Audience,
                                         claims,
                                         expires: dateTimeService.UtcNow.AddMinutes(options.LifetimeMinutes),
                                         signingCredentials: credentials);

        var stringToken = new JwtSecurityTokenHandler().WriteToken(token);

        return stringToken;
    }
}