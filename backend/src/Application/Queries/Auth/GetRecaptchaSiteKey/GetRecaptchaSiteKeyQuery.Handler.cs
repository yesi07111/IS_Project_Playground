using Microsoft.Extensions.Configuration;
using Playground.Application.Responses;

namespace Playground.Application.Queries.Auth.GetRecaptchaSiteKey;

public class GetRecaptchaSiteKeyQueryHandler
{
    private readonly string _siteKey;

    public GetRecaptchaSiteKeyQueryHandler(IConfiguration configuration)
    {
        _siteKey = configuration["Recaptcha:SiteKey"]!;
    }

    public async Task<ReCaptchaSiteKeyResponse> ExecuteAsync(CancellationToken ct = default)
    {
        return await Task.FromResult(new ReCaptchaSiteKeyResponse(_siteKey));
    }
}