using Playground.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;

namespace Playground.Application.Services;

/// <summary>
/// Interfaz para un servicio de envío de correos electrónicos.
/// Extiende la funcionalidad de <see cref="IEmailSender{TUser}"/> para usuarios.
/// </summary>
public interface IEmailSenderService : IEmailSender<User> { }