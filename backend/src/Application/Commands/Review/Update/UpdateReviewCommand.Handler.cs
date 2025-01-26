using System.Runtime.InteropServices;
using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Domain.Entities;
using Playground.Domain.Specifications;

namespace Playground.Application.Commands.Review.Update;

public class UpdateReviewCommandHandler(IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork) : CommandHandler<UpdateReviewCommand, GenericResponse>
{
    public override async Task<GenericResponse> ExecuteAsync(UpdateReviewCommand command, CancellationToken ct)
    {
        var activityDateRepository = repositoryFactory.CreateRepository<ActivityDate>();
        var reviewRepository = repositoryFactory.CreateRepository<Domain.Entities.Review>();
        var userRepository = repositoryFactory.CreateRepository<Domain.Entities.Auth.User>();

        var parent = await userRepository.GetByIdAsync(command.UserId);

        if (parent is null)
        {
            ThrowError("No se encontró al usuario padre.");
        }

        var activityDate = await activityDateRepository.GetByIdAsync(Guid.Parse(command.ActivityDateId));

        if (activityDate is null)
        {
            ThrowError("No se encontró la fecha de la actividad asociada a la reseña.");
        }

        var reviewSpecification = ReviewSpecification.ByParent(command.UserId).And(ReviewSpecification.ByActivityDate(Guid.Parse(command.ActivityDateId)));
        var review = (await reviewRepository.GetBySpecificationAsync(reviewSpecification)).FirstOrDefault();

        if (review is null)
        {
            ThrowError("No se encontró la reseña.");
        }

        review.Comments = command.Comment;
        review.Score = command.Rating;

        reviewRepository.Update(review);
        unitOfWork.Commit();

        return new GenericResponse("Reseña Actualizada");
    }
}