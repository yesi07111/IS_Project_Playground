using Playground.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;

namespace Playground.Application.Services;

public interface IEmailSenderService : IEmailSender<User> { }