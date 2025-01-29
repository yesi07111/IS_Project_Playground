using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Domain.Entities;
using Playground.Domain.SmartEnum;

namespace Playground.Application.Commands.Review.Delete;

public class DeleteReviewCommandHandler(IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork) : CommandHandler<DeleteReviewCommand, GenericResponse>
{
    public override async Task<GenericResponse> ExecuteAsync(DeleteReviewCommand command, CancellationToken ct)
    {
        var reviewRepository = repositoryFactory.CreateRepository<Domain.Entities.Review>();
        var isUseCase = UseCaseSmartEnum.TryFromName(command.UseCase, out UseCaseSmartEnum useCase);

        var review = await reviewRepository.GetByIdAsync(Guid.Parse(command.ReviewId)) ?? throw new ArgumentException("La reseña no existe.");

        // Si se especifica un caso de uso
        if (isUseCase)
        {
            if (useCase == UseCaseSmartEnum.SoftDelete)
            {
                reviewRepository.MarkDeleted(review);
                unitOfWork.Commit();

                return new GenericResponse("Reseña Eliminada");
            }
            else if (useCase == UseCaseSmartEnum.Delete)
            {
                reviewRepository.Delete(review);
                unitOfWork.Commit();

                return new GenericResponse("Reseña Eliminada Permanentemente");
            }

            throw new ArgumentException("No se especifico un caso de uso válido.");
        }

        throw new ArgumentException("No se especifico un caso de uso.");
    }
}