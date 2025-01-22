using Playground.Application.Responses;
using Microsoft.Extensions.Configuration;

namespace Playground.Application.Queries.Auth.GetGoogleClientId;

public class GetGoogleClientIdQueryHandler
{
    private readonly IConfiguration _configuration;

    public GetGoogleClientIdQueryHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<GoogleClientIdResponse> ExecuteAsync(CancellationToken ct)
    {
        var clientId = _configuration["Google:ClientId"]!;
        return await Task.FromResult(new GoogleClientIdResponse(clientId));
    }
}