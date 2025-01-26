using Ardalis.SmartEnum;
using FastEndpoints;
using Playground.Application.Factories;
using Playground.Application.Dtos;
using Playground.Application.Responses;
using Playground.Domain.Entities;
using Playground.Domain.SmartEnum;
using Playground.Domain.Specifications;

namespace Playground.Application.Queries.Review.List;

/// <summary>
/// Manejador para la consulta de listado de actividades.
/// </summary>
public class ListReviewQueryHandler(IRepositoryFactory _repositoryFactory) : CommandHandler<ListReviewQuery, ListReviewResponse>
{

    /// <summary>
    /// Ejecuta la consulta para listar actividades según los filtros proporcionados.
    /// </summary>
    /// <param name="query">Consulta que contiene los filtros.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Una respuesta con la lista de actividades y tipos de actividades.</returns>
    public override async Task<ListReviewResponse> ExecuteAsync(ListReviewQuery query, CancellationToken ct = default)
    {
        // Crear repositorios usando el factory
        var activityRepository = _repositoryFactory.CreateRepository<Domain.Entities.Activity>();
        var activityDateRepository = _repositoryFactory.CreateRepository<ActivityDate>();
        var reviewRepository = _repositoryFactory.CreateRepository<Domain.Entities.Review>();
        var isUseCase = SmartEnum<UseCaseSmartEnum>.TryFromName(query.UseCase, out UseCaseSmartEnum useCase);

        IEnumerable<object> Result = [];

        // Si se especifica un caso de uso
        if (isUseCase)
        {
            // Ver si se solicita el resultado para la vista de Mis Reseñas
            if (useCase == UseCaseSmartEnum.MyReviewView)
            {
                if (!string.IsNullOrEmpty(query.UserId))
                {
                    var reservationRepository = _repositoryFactory.CreateRepository<Domain.Entities.Reservation>();
                    var reservationSpecification = ReservationSpecification.ByParent(query.UserId).And(ReservationSpecification.ByReservationState(ReservationStateSmartEnum.Completada.Name));
                    var reservations = await reservationRepository.GetBySpecificationAsync(
                        reservationSpecification,
                        r => r.ActivityDate,
                        r => r.ActivityDate.Activity
                    );

                    var reviewDtos = new Dictionary<string, ReviewDto>();

                    foreach (var reservation in reservations)
                    {
                        var reviewSpecification = ReviewSpecification.ByParent(query.UserId).And(ReviewSpecification.ByActivityDate(reservation.ActivityDate.Id));
                        var review = await reviewRepository.GetBySpecificationAsync(reviewSpecification, r => r.ActivityDate, r => r.ActivityDate.Activity);

                        var reviewDto = new ReviewDto
                        {
                            ReviewId = review.Any() ? review.First().Id.ToString() : "",
                            ActivityId = reservation.ActivityDate.Id.ToString(),
                            ActivityName = reservation.ActivityDate.Activity.Name,
                            Comment = review.Any() ? review.First().Comments : string.Empty,
                            Rating = review.Any() ? review.First().Score : -1
                        };

                        // Añadir o actualizar el diccionario
                        reviewDtos[reviewDto.ActivityId] = reviewDto;
                    }

                    Result = reviewDtos.Values.ToList();
                }

            }

            return new ListReviewResponse(Result);
        }

        throw new ArgumentException("No se especifico un caso de uso válido.");
    }
}
