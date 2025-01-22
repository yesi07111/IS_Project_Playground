using FastEndpoints;
using Playground.Application.Responses;

namespace Playground.Application.Queries.Auth.DecodeToken;

public record DecodeTokenQuery(string Token) : ICommand<DecodedTokenResponse>;