using FastEndpoints;
using Playground.Application.Factories;
using Playground.Application.Dtos;
using Playground.Application.Responses;
using Playground.Domain.Specifications;
using Playground.Domain.Specifications.BaseSpecifications;
using Playground.Domain.SmartEnum;
using Ardalis.SmartEnum;

namespace Playground.Application.Queries.User.List;

public class ListUserQueryHandler : CommandHandler<ListUserQuery, ListUserResponse>
{
    private readonly IRepositoryFactory _repositoryFactory;

    public ListUserQueryHandler(IRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public override async Task<ListUserResponse> ExecuteAsync(ListUserQuery query, CancellationToken ct = default)
    {
        var userRepository = _repositoryFactory.CreateRepository<Domain.Entities.Auth.User>();
        IEnumerable<Domain.Entities.Auth.User> users = [];

        var isUseCase = SmartEnum<UseCaseSmartEnum>.TryFromName(query.UseCase, out UseCaseSmartEnum useCase);
        if (isUseCase)
        {
            if (useCase == UseCaseSmartEnum.AsFilter)
            {
                if (AreAllPropertiesNull(query))
                {
                    users = await userRepository.GetAllAsync(u => u.Rol);
                }
                else
                {
                    var userSpecification = BuildUserSpecification(query);
                    users = await userRepository.GetBySpecificationAsync(userSpecification, u => u.Rol);
                }

                var userDtos = MapUsersToDtos(users);
                return new ListUserResponse(userDtos);
            }
        }

        throw new ArgumentException("No se especifico un caso de uso válido.");
    }

    private ISpecification<Domain.Entities.Auth.User> BuildUserSpecification(ListUserQuery query)
    {
        ISpecification<Domain.Entities.Auth.User> specification = new UserSpecification(user => true);

        if (!string.IsNullOrEmpty(query.Username))
        {
            specification = specification.And(UserSpecification.ByUserName(query.Username));
        }

        if (!string.IsNullOrEmpty(query.Email))
        {
            specification = specification.And(UserSpecification.ByEmail(query.Email));
        }

        if (!string.IsNullOrEmpty(query.EmailConfirmed))
        {
            if (bool.TryParse(query.EmailConfirmed, out bool emailConfirmed))
            {
                specification = specification.And(UserSpecification.ByEmailConfirmed(emailConfirmed));
            }
        }

        if (!string.IsNullOrEmpty(query.FirstName))
        {
            specification = specification.And(UserSpecification.ByFirstName(query.FirstName));
        }

        if (!string.IsNullOrEmpty(query.LastName))
        {
            specification = specification.And(UserSpecification.ByLastName(query.LastName));
        }

        if (!string.IsNullOrEmpty(query.Rol))
        {
            specification = specification.And(UserSpecification.ByRol(query.Rol));
        }

        if (query.MarkDeleted.HasValue && query.MarkDeleted.Value)
        {
            specification = specification.And(new UserSpecification(user => user.DeletedAt != null));
        }

        return specification;
    }

    private static bool AreAllPropertiesNull(ListUserQuery query)
    {
        return string.IsNullOrEmpty(query.Username) &&
               string.IsNullOrEmpty(query.Email) &&
               string.IsNullOrEmpty(query.EmailConfirmed) &&
               string.IsNullOrEmpty(query.FirstName) &&
               string.IsNullOrEmpty(query.LastName) &&
               string.IsNullOrEmpty(query.Rol) &&
               query.MarkDeleted is null;
    }

    private static IEnumerable<UserDto> MapUsersToDtos(IEnumerable<Domain.Entities.Auth.User> users)
    {
        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.UserName!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            Rol = user.Rol.Name
        }).ToList();
    }
}