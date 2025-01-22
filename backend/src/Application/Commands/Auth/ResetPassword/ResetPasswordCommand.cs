using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Commands.Auth.ResetPassword;

public record ResetPasswordCommand(string Identifier, string ReducedCode, string FullCode, string NewPassword) : ICommand<UserCreationResponse>;