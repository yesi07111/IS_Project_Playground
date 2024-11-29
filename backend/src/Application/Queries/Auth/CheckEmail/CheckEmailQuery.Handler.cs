using FastEndpoints;
using Playground.Application.Factories;
using Playground.Application.Queries.Dtos;
using Playground.Application.Repositories;
using Playground.Domain.Entities.Auth;

namespace Playground.Application.Queries.CheckEmail;

public class CheckEmailQueryHandler : CommandHandler<CheckEmailQuery, CheckEmailResponse>
{
    private readonly IRepository<User> _userRepository;

    public CheckEmailQueryHandler(IRepositoryFactory repositoryFactory)
    {
        _userRepository = repositoryFactory.CreateRepository<User>();
    }

    public override async Task<CheckEmailResponse> ExecuteAsync(CheckEmailQuery query, CancellationToken ct = default)
    {
        var user = await _userRepository.GetByIdAsync(query.Id);

        if (user == null)
        {
            ThrowError($"No se encontró al usuario con id {query.Id}");
        }

        if (user.UserName != query.UserName)
        {
            ThrowError($"No coincide el nombre del usuario: {user.UserName} con el proporcionado: {query.UserName}");

        }

        var IsVerified = user.EmailConfirmed;

        return new CheckEmailResponse(IsVerified);
    }
}
