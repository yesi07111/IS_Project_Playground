using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Domain.SmartEnum;

namespace Playground.Application.Commands.Reservation.Update;

public class UpdateReservationCommandHandler : CommandHandler<UpdateReservationCommand, GenericResponse>
{
    private readonly IRepositoryFactory repositoryFactory;
    private readonly IUnitOfWork unitOfWork;

    public UpdateReservationCommandHandler(IRepositoryFactory _repositoryFactory, IUnitOfWork _unitOfWork)
    {
        repositoryFactory = _repositoryFactory;
        unitOfWork = _unitOfWork;
    }

    public override async Task<GenericResponse> ExecuteAsync(UpdateReservationCommand command, CancellationToken ct)
    {
        var reservationRepository = repositoryFactory.CreateRepository<Domain.Entities.Reservation>();

        var reservation = await reservationRepository.GetByIdAsync(Guid.Parse(command.ReservationId));
        if (reservation is null)
        {
            ThrowError("Reservación no encontrada.");
        }
        else
        {
            if (command.State == ReservationStateSmartEnum.Cancelada.Name)
            {
                reservation.ReservationState = ReservationStateSmartEnum.Cancelada.Name;
            }
            else if (command.State == ReservationStateSmartEnum.Confirmada.Name)
            {
                reservation.ReservationState = ReservationStateSmartEnum.Confirmada.Name;
            }
            else if (command.State == ReservationStateSmartEnum.Completada.Name)
            {
                reservation.ReservationState = ReservationStateSmartEnum.Completada.Name;
            }
            else
            {
                ThrowError("No es válida la edición de una reserva si no es para cancelar, confirmar o completar");
            }

            reservationRepository.Update(reservation);
            await unitOfWork.CommitAsync();
        }

        return new GenericResponse("Reservación modificada");
    }
}