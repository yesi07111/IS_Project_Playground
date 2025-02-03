using Playground.Domain.Entities.Auth;

namespace Playground.Application.Services;
public interface IAccessFailedService
{
    Task IncrementAccessFailedCountAsync(string identifier);
    Task<int?> GetLockoutTimeRemainingAsync(User user);
    int MaxFailedAccessAttempts { get; }
    int LockoutDurationInMinutes { get; }
}