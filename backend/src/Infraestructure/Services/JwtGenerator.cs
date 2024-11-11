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

public class JwtGenerator : IJwtGenerator
{
    private readonly JwtConfiguration options;
    private readonly UserManager<User> userManager;
    private readonly IDateTimeService dateTimeService;

    public JwtGenerator(IOptions<JwtConfiguration> _options, UserManager<User> _userManager, IDateTimeService _dateTimeService)
    {
        options = _options.Value;
        userManager = _userManager;
        dateTimeService = _dateTimeService;
    }

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