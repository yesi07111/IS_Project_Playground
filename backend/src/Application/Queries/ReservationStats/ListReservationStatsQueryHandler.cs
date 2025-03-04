using FastEndpoints;
using Playground.Application.Factories;
using Playground.Application.Queries.Responses;
using Playground.Application.Repositories;

namespace Playground.Application.Queries.ReservationStats.List;

public class ListReservationStatsQueryHandler : CommandHandler<ListReservationStatsQuery, ListReservationStatsResponse>
{
    private readonly IRepositoryFactory _repositoryFactory;

    public ListReservationStatsQueryHandler(IRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public override async Task<ListReservationStatsResponse> ExecuteAsync(ListReservationStatsQuery query, CancellationToken ct = default)
    {
        var reservationRepository = _repositoryFactory.CreateRepository<Domain.Entities.Reservation>();
        IEnumerable<object> reservations = [];

        if (query.UseCase == "ActDateResFreq")
        {
            reservations = await GetAllActDateResFreq(reservationRepository);
        }
        else if (query.UseCase == "ActDateResFreqAgeRangeWithState")
        {
            reservations = await GetAllActDateResFreqAgeRangeWithState(reservationRepository);
        }

        return new ListReservationStatsResponse(reservations);
    }

    private async Task<IEnumerable<object>> GetAllActDateResFreq(IRepository<Domain.Entities.Reservation> reservationRepository)
    {
        var reservations = await reservationRepository.GetAllAsync(r => r.ActivityDate, r => r.ActivityDate.Activity);

        var result = reservations
            .GroupBy(r => r.ActivityDate.Id) // Agrupar por el ID de la fecha de la actividad
            .Select(g => new
            {
                ActivityDateInfo = $"{g.First().ActivityDate.Activity.Name} - {g.First().ActivityDate.DateTime:yyyy-MM-dd}", // Nombre y fecha
                ReservationCount = g.Count() // Cantidad de reservas
            })
            .ToList<object>(); // Convertir a lista de objetos

        return result;
    }

    private async Task<IEnumerable<object>> GetAllActDateResFreqAgeRangeWithState(IRepository<Domain.Entities.Reservation> reservationRepository)
    {
        var reservations = await reservationRepository.GetAllAsync(r => r.ActivityDate, r => r.ActivityDate.Activity);

        var ageRanges = new Dictionary<(int Min, int Max), string>
        {
            {(0, 3), "Infantes (0-3)"},
            {(4, 6), "Niños pequeños (4-6)"},
            {(7, 9), "Niños grandes (7-9)"},
            {(10, 12), "Pre-adolescentes (10-12)"}
        };

        var result = reservations
            .GroupBy(r => r.ReservationState) // Agrupar por estado de reservación
            .Select(g => (object)new // Convertir a object
            {
                State = g.Key, // Estado de la reservación
                AgeRanges = ageRanges.Select(range => new
                {
                    AgeGroup = range.Value,
                    Count = g.Count(r => r.ActivityDate.Activity.RecommendedAge >= range.Key.Min
                                      && r.ActivityDate.Activity.RecommendedAge <= range.Key.Max)
                }).Where(t => t.Count > 0).ToList() // Filtrar los rangos con al menos una reservación
            })
            .ToList();

        return result;
    }
}
