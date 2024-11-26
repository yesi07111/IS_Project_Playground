using FastEndpoints;
using Playground.Application.Commands.Dtos;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Domain.Entities.Auth;
using Playground.Domain.Specifications;

namespace Playground.Application.Commands.CleanUp
{
    public class CleanUpUnverifiedUsersCommandHandler : CommandHandler<CleanUpUnverifiedUsersCommand, CleanUpUnverifiedUsersResponse>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CleanUpUnverifiedUsersCommandHandler(IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork)
        {
            _userRepository = repositoryFactory.CreateRepository<User>();
            _unitOfWork = unitOfWork;
        }

        public override async Task<CleanUpUnverifiedUsersResponse> ExecuteAsync(CleanUpUnverifiedUsersCommand command, CancellationToken ct = default)
        {
            var unverifiedUsersSpecification = UserSpecification.ByEmailConfirmed(false)
                .And(UserSpecification.ByCreatedAt(DateTime.UtcNow.AddDays(-7), "less"))
                .And(UserSpecification.ByIsDeleted(false));

            var users = (await _userRepository.GetBySpecificationAsync(unverifiedUsersSpecification)).ToList();

            foreach (var user in users)
            {
                _userRepository.MarkDeleted(user);
            }

            await _unitOfWork.CommitAsync();

            return new CleanUpUnverifiedUsersResponse(users.Count);
        }
    }
}