using System.Security.Claims;
using Playground.Domain.Entities.Auth;

namespace Playground.Application.Services;

public interface IActiveSession
{
    string? UserId();
    Task<User?> GetActiveUser();
    string BaseUrl();
    ClaimsPrincipal? GetClaimPrincipal();
}