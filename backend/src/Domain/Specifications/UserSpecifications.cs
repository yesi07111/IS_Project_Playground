using System.Linq.Expressions;
using Playground.Domain.Entities.Auth;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Domain.Specifications
{
    /// <summary>
    /// Especificación para filtrar entidades de usuario.
    /// Proporciona métodos estáticos para crear especificaciones basadas en diferentes propiedades del usuario.
    /// </summary>
    public class UserSpecification : ISpecification<User>
    {
        private readonly Expression<Func<User, bool>> _expression;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UserSpecification"/>.
        /// </summary>
        /// <param name="expression">La expresión lambda que define la especificación.</param>
        public UserSpecification(Expression<Func<User, bool>> expression)
        {
            _expression = expression;
        }

        /// <summary>
        /// Convierte la especificación en una expresión lambda.
        /// </summary>
        /// <returns>Una expresión lambda que representa la especificación.</returns>
        public Expression<Func<User, bool>> ToExpression()
        {
            return _expression;
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica AND.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<User> And(ISpecification<User> other)
        {
            return new AndSpecification<User>(this, other);
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica OR.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<User> Or(ISpecification<User> other)
        {
            return new OrSpecification<User>(this, other);
        }

        /// <summary>
        /// Invierte la especificación actual utilizando una operación lógica NOT.
        /// </summary>
        /// <returns>Una nueva especificación que representa la inversión.</returns>
        public ISpecification<User> Not()
        {
            return new NotSpecification<User>(this);
        }

        /// <summary>
        /// Combina la especificación actual con la negación de otra especificación utilizando una operación lógica AND.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<User> AndNot(ISpecification<User> other)
        {
            return new AndSpecification<User>(this, new NotSpecification<User>(other));
        }

        /// <summary>
        /// Combina la especificación actual con la negación de otra especificación utilizando una operación lógica OR.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<User> OrNot(ISpecification<User> other)
        {
            return new OrSpecification<User>(this, new NotSpecification<User>(other));
        }


        /// <summary>
        /// Crea una especificación para filtrar usuarios por su primer nombre.
        /// </summary>
        /// <param name="firstName">El primer nombre del usuario.</param>
        /// <returns>Una especificación que filtra por primer nombre.</returns>
        public static UserSpecification ByFirstName(string firstName)
        {
            return new UserSpecification(user => user.FirstName == firstName);
        }

        /// <summary>
        /// Crea una especificación para filtrar usuarios por su apellido.
        /// </summary>
        /// <param name="lastName">El apellido del usuario.</param>
        /// <returns>Una especificación que filtra por apellido.</returns>
        public static UserSpecification ByLastName(string lastName)
        {
            return new UserSpecification(user => user.LastName == lastName);
        }

        /// <summary>
        /// Crea una especificación para filtrar usuarios por su código completo.
        /// </summary>
        /// <param name="fullCode">El código completo del usuario.</param>
        /// <returns>Una especificación que filtra por código completo.</returns>
        public static UserSpecification ByFullCode(string fullCode)
        {
            return new UserSpecification(user => user.FullCode == fullCode);
        }

        /// <summary>
        /// Crea una especificación para filtrar usuarios por su nombre de usuario.
        /// </summary>
        /// <param name="userName">El nombre de usuario.</param>
        /// <returns>Una especificación que filtra por nombre de usuario.</returns>
        public static UserSpecification ByUserName(string userName)
        {
            return new UserSpecification(user => user.UserName == userName);
        }

        /// <summary>
        /// Crea una especificación para filtrar usuarios por su correo electrónico.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <returns>Una especificación que filtra por correo electrónico.</returns>
        public static UserSpecification ByEmail(string email)
        {
            return new UserSpecification(user => user.Email == email);
        }

        /// <summary>
        /// Crea una especificación para filtrar usuarios por el estado de confirmación de su correo electrónico.
        /// </summary>
        /// <param name="emailConfirmed">Indica si el correo electrónico está confirmado.</param>
        /// <returns>Una especificación que filtra por estado de confirmación de correo electrónico.</returns>
        public static UserSpecification ByEmailConfirmed(bool emailConfirmed)
        {
            return new UserSpecification(user => user.EmailConfirmed == emailConfirmed);
        }

        /// <summary>
        /// Crea una especificación para filtrar usuarios por su hash de contraseña.
        /// </summary>
        /// <param name="passwordHash">El hash de la contraseña del usuario.</param>
        /// <returns>Una especificación que filtra por hash de contraseña.</returns>
        public static UserSpecification ByPasswordHash(string passwordHash)
        {
            return new UserSpecification(user => user.PasswordHash == passwordHash);
        }

        /// <summary>
        /// Crea una especificación para filtrar usuarios por su rol.
        /// </summary>
        /// <param name="rolName">El nombre del rol del usuario.</param>
        /// <returns>Una especificación que filtra por rol de usuario.</returns>
        public static UserSpecification ByRol(string rolName)
        {
            if (rolName is null) return new UserSpecification(r => r.Rol == null);
            return new UserSpecification(user => user.Rol.Name == rolName);
        }

        /// <summary>
        /// Crea una especificación para filtrar usuarios por su fecha de creación.
        /// </summary>
        /// <param name="createdAt">La fecha de creación del usuario.</param>
        /// <param name="comparison">El tipo de comparación a realizar (por defecto es "equal").</param>
        /// <returns>Una especificación que filtra por fecha de creación.</returns>
        public static UserSpecification ByCreatedAt(DateTime createdAt, string comparison = "equal")
        {
            return new UserSpecification(DateTimeSpecification<User>.CreateDateComparisonExpression(user => user.CreatedAt, createdAt, comparison));
        }

        /// <summary>
        /// Crea una especificación para filtrar usuarios por su fecha de actualización.
        /// </summary>
        /// <param name="updateAt">La fecha de actualización del usuario.</param>
        /// <param name="comparison">El tipo de comparación a realizar (por defecto es "equal").</param>
        /// <returns>Una especificación que filtra por fecha de actualización.</returns>
        public static UserSpecification ByUpdateAt(DateTime updateAt, string comparison = "equal")
        {
            return new UserSpecification(DateTimeSpecification<User>.CreateDateComparisonExpression(user => user.UpdateAt, updateAt, comparison));
        }

        /// <summary>
        /// Crea una especificación para filtrar usuarios por su fecha de eliminación.
        /// </summary>
        /// <param name="deleteAt">La fecha de eliminación del usuario.</param>
        /// <param name="comparison">El tipo de comparación a realizar (por defecto es "equal").</param>
        /// <returns>Una especificación que filtra por fecha de eliminación.</returns>
        public static UserSpecification ByDeletedAt(DateTime? deleteAt, string comparison = "equal")
        {
            return new UserSpecification(DateTimeSpecification<User>.CreateNullableDateComparisonExpression(user => user.DeletedAt, deleteAt, comparison));
        }
    }
}