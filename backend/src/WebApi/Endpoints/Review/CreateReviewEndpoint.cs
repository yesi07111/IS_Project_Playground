using FastEndpoints;
using Playground.Application.Commands.Responses;
using Playground.Application.Commands.Review.Create;

namespace Playground.WebApi.Endpoints.Review;

public class CreateReviewEndpoint : Endpoint<CreateReviewCommand, GenericResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/review/create");
    }

    public override async Task<GenericResponse> ExecuteAsync(CreateReviewCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}