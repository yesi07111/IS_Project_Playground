using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Application.Responses;
using Playground.Domain.SmartEnum;

namespace Playground.Application.Commands.Reservation.Create;

public class ReserveActivityCommandHandler(UserManager<Domain.Entities.Auth.User> userManager, IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork) : CommandHandler<ReserveActivityCommand, ReservationCreationResponse>
{
    public override async Task<ReservationCreationResponse> ExecuteAsync(ReserveActivityCommand command, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(command.UserId);
        if (user == null)
        {
            ThrowError("Usuario no encontrado.");
        }

        var activityDateRepository = repositoryFactory.CreateRepository<Domain.Entities.ActivityDate>();
        var activityDate = await activityDateRepository.GetByIdAsync(Guid.Parse(command.ActivityId));

        if (activityDate == null)
        {
            ThrowError("Actividad no encontrada.");
        }

        var reservationRepository = repositoryFactory.CreateRepository<Domain.Entities.Reservation>();

        var reservation = new Domain.Entities.Reservation
        {
            Parent = user,
            ActivityDate = activityDate,
            AmmountOfChildren = command.Amount,
            AdditionalComments = command.Comments,
            ReservationState = ReservationStateSmartEnum.Pendiente.Name
        };

        await reservationRepository.AddAsync(reservation);
        await unitOfWork.CommitAsync();
        return new ReservationCreationResponse(true);
    }
}