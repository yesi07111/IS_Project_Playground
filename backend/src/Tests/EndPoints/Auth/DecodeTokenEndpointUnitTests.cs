    using FastEndpoints;
    using Playground.Application.Queries.Auth.DecodeToken;
    using Playground.Application.Responses;

    namespace Playground.WebApi.Endpoints.Auth;

    public class DecodeTokenEndpoint : Endpoint<DecodeTokenQuery, DecodedTokenResponse>
    {
        public override void Configure()
        {
            AllowAnonymous();
            Get("/auth/decode-token");
        }

        public override async Task<DecodedTokenResponse> ExecuteAsync(DecodeTokenQuery req, CancellationToken ct)
        {
            return await req.ExecuteAsync(ct);
        }
    }