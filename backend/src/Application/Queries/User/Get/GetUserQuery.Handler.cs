
using Ardalis.SmartEnum;
using FastEndpoints;
using Playground.Application.Factories;
using Playground.Application.Dtos;
using Playground.Application.Responses;
using Playground.Domain.SmartEnum;

namespace Playground.Application.Queries.User.Get;

/// <summary>
/// Manejador para la consulta de obtener una actividad.
/// </summary>
public class GetUserQueryHandler(IRepositoryFactory repositoryFactory) : CommandHandler<GetUserQuery, GetUserResponse>
{
    /// <summary>
    /// Ejecuta la consulta para Getar actividades según los filtros proporcionados.
    /// </summary>
    /// <param name="query">Consulta que contiene los filtros.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Una respuesta con la Geta de actividades y tipos de actividades.</returns>
    public override async Task<GetUserResponse> ExecuteAsync(GetUserQuery query, CancellationToken ct = default)
    {
        var isUseCase = SmartEnum<UseCaseSmartEnum>.TryFromName(query.UseCase, out UseCaseSmartEnum useCase);
        var userDto = new UserDto();
        var userRepository = repositoryFactory.CreateRepository<Domain.Entities.Auth.User>();
        var user = await userRepository.GetByIdAsync(query.Id, u => u.Rol)
        ?? throw new KeyNotFoundException("El usuario no fue encontrado.");

        if (isUseCase)
        {
            if (useCase == UseCaseSmartEnum.UserProfileView)
            {

                userDto = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.UserName ?? "",
                    Email = user.Email ?? "",
                    Rol = user.Rol.Name
                };
            }

            return new GetUserResponse(userDto);
        }

        throw new ArgumentException("No se especifico un caso de uso válido.");
    }

}