using FastEndpoints;
using Playground.Application.Commands.Auth.UploadUserImage;
using Playground.Application.Responses;

public class UploadUserImageEndpoint : Endpoint<UploadUserImageCommand, UserImageUploadResponse>
{
    public override void Configure()
    {
        Post("/auth/upload-user-image");
        AllowAnonymous();
        AllowFileUploads();
    }

    public override async Task<UserImageUploadResponse> ExecuteAsync(UploadUserImageCommand req, CancellationToken ct)
    {
        return await req.ExecuteAsync(ct);
    }
}