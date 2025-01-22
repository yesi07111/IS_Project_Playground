using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Playground.Application.Responses;

namespace Playground.Application.Commands.Auth.UploadUserImage;

public class UploadUserImageCommand : ICommand<UserImageUploadResponse>
{
    public string Username { get; set; }
    public IFormFile? Image { get; set; }
}