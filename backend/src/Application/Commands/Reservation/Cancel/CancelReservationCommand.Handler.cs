using FastEndpoints;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Application.Responses;
using Playground.Application.Services;
using Playground.Domain.SmartEnum;
using Playground.Domain.Specifications;

namespace Playground.Application.Commands.Reservation.Cancel;

public class CancelReservationCommandHandler(IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork, IJwtGenerator jwtGenerator) : CommandHandler<CancelReservationCommand, UserActionResponse>
{
    public override async Task<UserActionResponse> ExecuteAsync(CancelReservationCommand command, CancellationToken ct = default)
    {
        var userRepository = repositoryFactory.CreateRepository<Domain.Entities.Auth.User>();
        var user = await userRepository.GetByIdAsync(command.UserId, user => user.Rol);
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
        var reservationSpecifications = ReservationSpecification.ByParent(user.Id).And(ReservationSpecification.ByActivityDate(activityDate.Id));
        var reservation = (await reservationRepository.GetBySpecificationAsync(reservationSpecifications)).FirstOrDefault();

        if (reservation is null)
        {
            ThrowError("Reserva no encontrada.");
        }

        if (reservation.ReservationState == ReservationStateSmartEnum.Confirmada.Name)
        {
            activityDate.ReservedPlaces = activityDate.ReservedPlaces - reservation.AmmountOfChildren;
            activityDateRepository.Update(activityDate);
        }
        reservation.ReservationState = ReservationStateSmartEnum.Cancelada.Name;
        reservationRepository.Update(reservation);
        unitOfWork.Commit();

        return new UserActionResponse(Guid.Parse(user.Id), user.UserName!, jwtGenerator.GetToken(user), user.Rol!.Name);
    }
}