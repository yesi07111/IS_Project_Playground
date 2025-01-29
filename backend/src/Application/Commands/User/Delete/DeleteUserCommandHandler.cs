using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Domain.Specifications;

namespace Playground.Application.Commands.User.Delete;

public class DeleteUserCommandHandler : CommandHandler<DeleteUserCommand, GenericResponse>
{
    private readonly IRepositoryFactory repositoryFactory;
    private readonly IUnitOfWork unitOfWork;

    public DeleteUserCommandHandler(IRepositoryFactory _repositoryFactory, IUnitOfWork _unitOfWork)
    {
        repositoryFactory = _repositoryFactory;
        unitOfWork = _unitOfWork;
    }

    public override async Task<GenericResponse> ExecuteAsync(DeleteUserCommand command, CancellationToken ct)
    {
        Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + command.Id);
        var userRepository = repositoryFactory.CreateRepository<Domain.Entities.Auth.User>();
        var reservationRepository = repositoryFactory.CreateRepository<Domain.Entities.Reservation>();
        var reviewRepository = repositoryFactory.CreateRepository<Domain.Entities.Review>();
        var activityRepository = repositoryFactory.CreateRepository<Domain.Entities.Activity>();

        var user = await userRepository.GetByIdAsync(command.Id) ?? throw new ArgumentException("El usuario no existe.");
        var reservations = await reservationRepository.GetBySpecificationAsync(ReservationSpecification.ByParent(user.Id));
        var reviews = await reviewRepository.GetBySpecificationAsync(ReviewSpecification.ByParent(user.Id));
        var activities = await activityRepository.GetBySpecificationAsync(ActivitySpecification.ByEducator(user.Id));

        if(reservations != null && reservations.Any())
        {
            foreach(var reservation in reservations)
            {
                reservationRepository.Delete(reservation);
            }
        }
        if(reviews != null && reviews.Any())
        {
            foreach(var review in reviews)
            {
                reviewRepository.Delete(review);
            }
        }
        if(activities != null && activities.Any())
        {
            foreach(var activity in activities)
            {
                activityRepository.Delete(activity);
            }
        }

        userRepository.Delete(user);
        unitOfWork.Commit();

        return new GenericResponse("Usuario Eliminado Permanentemente");
    }
}