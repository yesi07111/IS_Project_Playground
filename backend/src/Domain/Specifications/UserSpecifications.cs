namespace Playground.Domain.Specifications
{
    using Playground.Domain.Entities.Auth;
    using Playground.Domain.Specifications.BaseSpecifications;

    public class UserSpecification : ISpecification<User>
    {
        private readonly Func<User, bool> _expression;

        public UserSpecification(Func<User, bool> expression)
        {
            _expression = expression;
        }

        public bool IsSatisfiedBy(User user)
        {
            return _expression(user);
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

        public static UserSpecification ByFirstName(string firstName)
        {
            return new UserSpecification(user => user.FirstName == firstName);
        }

        public static UserSpecification ByLastName(string lastName)
        {
            return new UserSpecification(user => user.LastName == lastName);
        }

        public static UserSpecification ByFullCode(string fullCode)
        {
            return new UserSpecification(user => user.FullCode == fullCode);
        }

        public static UserSpecification ByCreatedAt(DateTime createdAt)
        {
            return new UserSpecification(user => user.CreatedAt == createdAt);
        }

        public static UserSpecification ByUpdateAt(DateTime updateAt)
        {
            return new UserSpecification(user => user.UpdateAt == updateAt);
        }

        public static UserSpecification ByDeleteAt(DateTime deleteAt)
        {
            return new UserSpecification(user => user.DeleteAt == deleteAt);
        }

        public static UserSpecification ByUserName(string userName)
        {
            return new UserSpecification(user => user.UserName == userName);
        }

        public static UserSpecification ByEmail(string email)
        {
            return new UserSpecification(user => user.Email == email);
        }

        public static UserSpecification ByEmailConfirmed(bool emailConfirmed)
        {
            return new UserSpecification(user => user.EmailConfirmed == emailConfirmed);
        }

        public static UserSpecification ByPasswordHash(string passwordHash)
        {
            return new UserSpecification(user => user.PasswordHash == passwordHash);
        }
    }
}