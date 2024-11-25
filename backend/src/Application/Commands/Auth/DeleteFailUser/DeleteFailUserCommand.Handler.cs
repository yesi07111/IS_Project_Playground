using FastEndpoints;
using Playground.Application.Repositories;
using Playground.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Commands.Users.DeleteFailUser;
using Playground.Application.Factories;
using Playground.Domain.Specifications;

namespace Playground.Application.Commands.DeleteFailUser;

public class DeleteFailUserCommandHandler : CommandHandler<DeleteFailUserCommand, DeleteFailUserResponse>
{
    private readonly IRepository<User> _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFailUserCommandHandler(UserManager<User> userManager, IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork)
    {
        _userRepository = repositoryFactory.CreateRepository<User>();
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public override async Task<DeleteFailUserResponse> ExecuteAsync(DeleteFailUserCommand command, CancellationToken ct = default)
    {
        var userSpecification = UserSpecification.ByDeleteToken(Guid.Parse(command.DeleteToken));
        var user = (await _userRepository.GetBySpecificationAsync(userSpecification)).FirstOrDefault();

        if (user == null)
        {
            return new DeleteFailUserResponse(false, "User not found");
        }

        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains(command.UserType))
        {
            return new DeleteFailUserResponse(false, "User type does not match");
        }

        _userRepository.MarkDeleted(user);
        await _unitOfWork.CommitAsync();

        return new DeleteFailUserResponse(true, "User marked as deleted successfully");
    }
}