using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Domain.Entities;

namespace Playground.Application.Commands.Review.Create;

public class CreateReviewCommandHandler(IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork) : CommandHandler<CreateReviewCommand, GenericResponse>
{
    public override async Task<GenericResponse> ExecuteAsync(CreateReviewCommand command, CancellationToken ct)
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

        var review = new Domain.Entities.Review
        {
            Parent = parent,
            ActivityDate = activityDate,
            Comments = command.Comment,
            Score = command.Rating,
        };

        await reviewRepository.AddAsync(review);
        await unitOfWork.CommitAsync();

        return new GenericResponse("Reseña Creada");
    }
}