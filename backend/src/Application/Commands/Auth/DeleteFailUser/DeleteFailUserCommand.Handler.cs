using FastEndpoints;
using Playground.Application.Repositories;
using Playground.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Commands.Users.DeleteFailUser;
using Playground.Application.Factories;

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
        Console.WriteLine(command);
        var user = await _userRepository.GetByIdAsync(command.Id);

        if (user == null)
        {
            return new DeleteFailUserResponse(false, "Usuario no encontrado.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        Console.WriteLine("El rol del usuario no coincide con el que se el pasa.", !roles.Contains(command.UserType));

        if (!roles.Contains(command.UserType))
        {
            return new DeleteFailUserResponse(false, "El tipo de usuario no es el mismo.");
        }

        _userRepository.Delete(user);
        await _unitOfWork.CommitAsync();

        Console.WriteLine("El usuario se borro");


        return new DeleteFailUserResponse(true, "El usuario se ha eliminado satisfactoriamente.");
    }
}