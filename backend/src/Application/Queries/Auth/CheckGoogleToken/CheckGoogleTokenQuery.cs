using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Queries.Auth.CheckGoogleToken;

public record CheckGoogleTokenQuery(string Token) : ICommand<GoogleTokenValidationResponse>;