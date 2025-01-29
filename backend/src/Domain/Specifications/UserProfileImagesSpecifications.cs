using System.Linq.Expressions;
using Playground.Domain.Entities;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Domain.Specifications
{
    /// <summary>
    /// Especificación para la entidad UserProfileImages.
    /// </summary>
    public class UserProfileImagesSpecification : ISpecification<UserProfileImages>
    {
        private readonly Expression<Func<UserProfileImages, bool>> _expression;

        /// <summary>
        /// Constructor privado para crear una especificación basada en una expresión.
        /// </summary>
        /// <param name="expression">La expresión que define la especificación.</param>
        private UserProfileImagesSpecification(Expression<Func<UserProfileImages, bool>> expression)
        {
            _expression = expression;
        }

        /// <summary>
        /// Convierte la especificación en una expresión lambda.
        /// </summary>
        /// <returns>Una expresión lambda que representa la especificación.</returns>
        public Expression<Func<UserProfileImages, bool>> ToExpression()
        {
            return _expression;
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica AND.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<UserProfileImages> And(ISpecification<UserProfileImages> other)
        {
            return new UserProfileImagesSpecification(Expression.Lambda<Func<UserProfileImages, bool>>(
                Expression.AndAlso(_expression.Body, other.ToExpression().Body),
                _expression.Parameters));
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica OR.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<UserProfileImages> Or(ISpecification<UserProfileImages> other)
        {
            return new UserProfileImagesSpecification(Expression.Lambda<Func<UserProfileImages, bool>>(
                Expression.OrElse(_expression.Body, other.ToExpression().Body),
                _expression.Parameters));
        }

        /// <summary>
        /// Invierte la especificación actual utilizando una operación lógica NOT.
        /// </summary>
        /// <returns>Una nueva especificación que representa la inversión.</returns>
        public ISpecification<UserProfileImages> Not()
        {
            return new UserProfileImagesSpecification(Expression.Lambda<Func<UserProfileImages, bool>>(
                Expression.Not(_expression.Body),
                _expression.Parameters));
        }

        /// <summary>
        /// Combina la especificación actual con la negación de otra especificación utilizando una operación lógica AND.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<UserProfileImages> AndNot(ISpecification<UserProfileImages> other)
        {
            return new AndSpecification<UserProfileImages>(this, new NotSpecification<UserProfileImages>(other));
        }

        /// <summary>
        /// Combina la especificación actual con la negación de otra especificación utilizando una operación lógica OR.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<UserProfileImages> OrNot(ISpecification<UserProfileImages> other)
        {
            return new OrSpecification<UserProfileImages>(this, new NotSpecification<UserProfileImages>(other));
        }


        /// <summary>
        /// Crea una especificación para filtrar UserProfileImages por el userId.
        /// </summary>
        /// <param name="userId">El identificador del usuario.</param>
        /// <returns>Una especificación que filtra por userId.</returns>
        public static UserProfileImagesSpecification ByUserId(string userId)
        {
            return new UserProfileImagesSpecification(image => image.User.Id == userId);
        }

        /// <summary>
        /// Crea una especificación para filtrar UserProfileImages por el path de la imagen de perfil.
        /// </summary>
        /// <param name="profileImagePath">El path de la imagen de perfil.</param>
        /// <returns>Una especificación que filtra por el path de la imagen de perfil.</returns>
        public static UserProfileImagesSpecification ByProfileImagePath(string profileImagePath)
        {
            return new UserProfileImagesSpecification(image => image.ProfileImage == profileImagePath);
        }

        /// <summary>
        /// Crea una especificación para filtrar UserProfileImages por el path de otras imágenes.
        /// </summary>
        /// <param name="otherImagePath">El path de la otra imagen.</param>
        /// <returns>Una especificación que filtra por el path de otras imágenes.</returns>
        public static UserProfileImagesSpecification ByOtherImagePath(string otherImagePath)
        {
            return new UserProfileImagesSpecification(image => image.OtherImages.Split(new[] { ',' }).Contains(otherImagePath));
        }
    }
}