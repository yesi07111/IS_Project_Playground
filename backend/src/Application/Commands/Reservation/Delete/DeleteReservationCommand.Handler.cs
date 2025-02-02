using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Domain.SmartEnum;

namespace Playground.Application.Commands.Reservation.Delete;

public class DeleteReservationCommandHandler : CommandHandler<DeleteReservationCommand, GenericResponse>
{
    private readonly IRepositoryFactory repositoryFactory;
    private readonly IUnitOfWork unitOfWork;

    public DeleteReservationCommandHandler(IRepositoryFactory _repositoryFactory, IUnitOfWork _unitOfWork)
    {
        repositoryFactory = _repositoryFactory;
        unitOfWork = _unitOfWork;
    }

    public override async Task<GenericResponse> ExecuteAsync(DeleteReservationCommand command, CancellationToken ct)
    {
        var reservationRepository = repositoryFactory.CreateRepository<Domain.Entities.Reservation>();

        var reservation = await reservationRepository.GetByIdAsync(Guid.Parse(command.Id));
        if (reservation is null)
        {
            ThrowError("Reservación no encontrada.");
        }
        else
        {
            if (reservation.ReservationState != ReservationStateSmartEnum.Completada.Name &&
            reservation.ReservationState != ReservationStateSmartEnum.Cancelada.Name)
            {
                ThrowError("No está permitido borrar reservas que no estén canceladas o completadas.");
            }
            else
            {
                reservationRepository.Delete(reservation);
                await unitOfWork.CommitAsync();
            }
        }

        return new GenericResponse("Reservación eliminada");
    }
}