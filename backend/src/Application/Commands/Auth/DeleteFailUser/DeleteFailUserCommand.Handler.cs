using FastEndpoints;
using Playground.Application.Repositories;
using Playground.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Factories;
using Playground.Application.Commands.Responses;

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
        System.Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
        System.Console.WriteLine("ENTRO AL DELETEFAIL");
        Console.WriteLine("Command: {0}", command);
        var user = await _userRepository.GetByIdAsync(command.Id, u => u.Rol);

        if (user == null || user.Rol.Name != command.UserType)
        {
            return new DeleteFailUserResponse(false, "Usuario no encontrado.");
        }
        System.Console.WriteLine("User firstname: {0}", user.FirstName);

        _userRepository.Delete(user);
        await _unitOfWork.CommitAsync();

        return new DeleteFailUserResponse(true, "El usuario se ha eliminado satisfactoriamente.");
    }
}