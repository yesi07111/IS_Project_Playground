using System.Linq.Expressions;
using Playground.Domain.Entities.Auth;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Domain.Specifications
{
    public class UserSpecification : ISpecification<User>
    {
        private readonly Expression<Func<User, bool>> _expression;

        public UserSpecification(Expression<Func<User, bool>> expression)
        {
            _expression = expression;
        }

        public Expression<Func<User, bool>> ToExpression()
        {
            return _expression;
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

        public static UserSpecification ByDeleteToken(Guid deleteToken)
        {
            return new UserSpecification(user => user.DeleteToken == deleteToken);
        }

        public static UserSpecification ByIsDeleted(bool isDeleted)
        {
            return new UserSpecification(user => user.IsDeleted == isDeleted);
        }

        public static UserSpecification ByRole(DefaultDbContext context, string roleName)
        {
            return new UserSpecification(user =>
                context.UserRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Select(ur => ur.Role)
                    .Any(role => role.Name == roleName));
        }

        public static UserSpecification ByCreatedAt(DateTime createdAt, string comparison = "equal")
        {
            return new UserSpecification(CreateDateComparisonExpression(user => user.CreatedAt, createdAt, comparison));
        }

        public static UserSpecification ByUpdateAt(DateTime updateAt, string comparison = "equal")
        {
            return new UserSpecification(CreateDateComparisonExpression(user => user.UpdateAt, updateAt, comparison));
        }

        public static UserSpecification ByDeleteAt(DateTime deleteAt, string comparison = "equal")
        {
            return new UserSpecification(CreateDateComparisonExpression(user => user.DeletedAt, deleteAt, comparison));
        }


        private static Expression<Func<User, bool>> CreateDateComparisonExpression(
                Expression<Func<User, DateTime>> dateSelector, DateTime date, string comparison)
        {
            switch (comparison)
            {
                case "greater":
                    return Expression.Lambda<Func<User, bool>>(
                        Expression.GreaterThan(dateSelector.Body, Expression.Constant(date)),
                        dateSelector.Parameters);
                case "greater-or-equal":
                    return Expression.Lambda<Func<User, bool>>(
                        Expression.GreaterThanOrEqual(dateSelector.Body, Expression.Constant(date)),
                        dateSelector.Parameters);
                case "less":
                    return Expression.Lambda<Func<User, bool>>(
                        Expression.LessThan(dateSelector.Body, Expression.Constant(date)),
                        dateSelector.Parameters);
                case "less-or-equal":
                    return Expression.Lambda<Func<User, bool>>(
                        Expression.LessThanOrEqual(dateSelector.Body, Expression.Constant(date)),
                        dateSelector.Parameters);
                default:
                    return Expression.Lambda<Func<User, bool>>(
                        Expression.Equal(dateSelector.Body, Expression.Constant(date)),
                        dateSelector.Parameters);
            }
        }

    }
}