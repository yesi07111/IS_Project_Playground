using Microsoft.AspNetCore.Identity;
using Playground.Domain.Entities.Auth;

namespace Playground.Application.Services
{
    public class AccessFailedService : IAccessFailedService
    {
        private readonly UserManager<User> _userManager;

        public AccessFailedService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public int MaxFailedAccessAttempts => 5;
        public int LockoutDurationInMinutes => 15;

        public async Task IncrementAccessFailedCountAsync(string identifier)
        {
            var user = await _userManager.FindByEmailAsync(identifier) ?? await _userManager.FindByNameAsync(identifier);
            if (user != null)
            {
                await _userManager.AccessFailedAsync(user);
                if (await _userManager.GetAccessFailedCountAsync(user) >= MaxFailedAccessAttempts)
                {
                    await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(LockoutDurationInMinutes));
                }
            }
        }

        public async Task<int?> GetLockoutTimeRemainingAsync(User user)
        {
            var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
            if (lockoutEnd.HasValue && lockoutEnd.Value > DateTimeOffset.UtcNow)
            {
                return (int)(lockoutEnd.Value - DateTimeOffset.UtcNow).TotalMinutes;
            }
            return null;
        }
    }
}