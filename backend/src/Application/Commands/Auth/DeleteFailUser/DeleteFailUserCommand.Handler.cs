using FastEndpoints;
using Playground.Application.Repositories;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Factories;
using Playground.Application.Responses;

namespace Playground.Application.Commands.DeleteFailUser;

public class DeleteFailUserCommandHandler : CommandHandler<DeleteFailUserCommand, DeleteFailUserResponse>
{
    private readonly IRepository<Domain.Entities.Auth.User> _userRepository;
    private readonly UserManager<Domain.Entities.Auth.User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFailUserCommandHandler(UserManager<Domain.Entities.Auth.User> userManager, IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork)
    {
        _userRepository = repositoryFactory.CreateRepository<Domain.Entities.Auth.User>();
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public override async Task<DeleteFailUserResponse> ExecuteAsync(DeleteFailUserCommand command, CancellationToken ct = default)
    {
        var user = await _userRepository.GetByIdAsync(command.Id, u => u.Rol);

        if (user == null || user.Rol.Name != command.UserType)
        {
            return new DeleteFailUserResponse(false, "Usuario no encontrado.");
        }

        _userRepository.Delete(user);
        await _unitOfWork.CommitAsync();

        return new DeleteFailUserResponse(true, "El usuario se ha eliminado satisfactoriamente.");
    }
}