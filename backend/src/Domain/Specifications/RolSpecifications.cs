namespace Playground.Domain.Specifications
{
    using Playground.Domain.Entities.Auth;
    using Playground.Domain.Specifications.BaseSpecifications;
    using Microsoft.AspNetCore.Identity;

    public class RoleSpecification : ISpecification<User>
    {
        private readonly UserManager<User> _userManager;
        private readonly string _roleName;

        public RoleSpecification(UserManager<User> userManager, string roleName)
        {
            _userManager = userManager;
            _roleName = roleName;
        }

        public bool IsSatisfiedBy(User user)
        {
            return _userManager.IsInRoleAsync(user, _roleName).Result;
        }

        public ISpecification<User> And(ISpecification<User> other)
        {
            return new AndSpecification<User>(this, other);
        }

        public ISpecification<User> Or(ISpecification<User> other)
        {
            return new OrSpecification<User>(this, other);
        }

        public ISpecification<User> Not()
        {
            return new NotSpecification<User>(this);
        }
    }
}