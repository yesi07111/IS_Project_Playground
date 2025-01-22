using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Application.Responses;
using Playground.Domain.Specifications;

namespace Playground.Application.Commands.CleanUp;

public class CleanUpUnverifiedUsersCommandHandler
{
    private readonly IRepository<Domain.Entities.Auth.User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CleanUpUnverifiedUsersCommandHandler(IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork)
    {
        _userRepository = repositoryFactory.CreateRepository<Domain.Entities.Auth.User>();
        _unitOfWork = unitOfWork;
    }

    public async Task<CleanUpUnverifiedUsersResponse> ExecuteAsync(CancellationToken ct = default)
    {
        var unverifiedUsersSpecification = UserSpecification.ByEmailConfirmed(false)
            .And(UserSpecification.ByCreatedAt(DateTime.UtcNow.AddDays(-7), "less"))
            .And(UserSpecification.ByDeletedAt(null));

        var users = (await _userRepository.GetBySpecificationAsync(unverifiedUsersSpecification)).ToList();

        foreach (var user in users)
        {
            _userRepository.MarkDeleted(user);
        }

        await _unitOfWork.CommitAsync();

        return new CleanUpUnverifiedUsersResponse(users.Count);
    }
}