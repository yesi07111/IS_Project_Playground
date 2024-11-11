using Playground.Domain.Entities.Auth;

namespace Playground.Application.Services;

public interface IJwtGenerator
{
    public Task<string> GetToken(User user);
}