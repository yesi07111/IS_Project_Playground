using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Playground.Application.Responses;
using Playground.Domain.Entities;
using Playground.Domain.Specifications;

namespace Playground.Application.Commands.Auth.UploadUserImage;

public class UploadUserImageCommandHandler(UserManager<Domain.Entities.Auth.User> userManager, IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork) : CommandHandler<UploadUserImageCommand, UserImageUploadResponse>
{
    public override async Task<UserImageUploadResponse> ExecuteAsync(UploadUserImageCommand command, CancellationToken ct)
    {
        var baseDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\"));

        var userImagesDir = Path.Combine(baseDir, "frontend", "public", "userImages");

        var userDir = Path.Combine(userImagesDir, command.Username);

        if (!Directory.Exists(userDir))
        {
            Directory.CreateDirectory(userDir);
        }

        var existingFiles = Directory.GetFiles(userDir);

        var user = await userManager.FindByNameAsync(command.Username);

        if (user == null)
        {
            ThrowError("Usuario no encontrado");
        }

        var userId = user.Id;
        var imagesRepository = repositoryFactory.CreateRepository<UserProfileImages>();
        var userProfileImagesSpec = UserProfileImagesSpecification.ByUserId(userId);
        var userProfileImages = (await imagesRepository.GetBySpecificationAsync(userProfileImagesSpec, image => image.User)).FirstOrDefault();

        if (userProfileImages == null)
        {
            userProfileImages = new UserProfileImages
            {
                User = user,
                OtherImages = string.Join(",", GetRelativePaths(existingFiles))
            };
            await imagesRepository.AddAsync(userProfileImages);
            await unitOfWork.CommitAsync();
        }

        string imagePath = string.Empty;

        if (command.Image != null && command.Image.Length > 0)
        {
            var filePath = Path.Combine(userDir, command.Image.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await command.Image.CopyToAsync(stream);
            }
            imagePath = GetRelativePath(filePath);
            userProfileImages.ProfileImage = imagePath;
        }
        else
        {
            imagePath = GetRelativePath(userProfileImages.ProfileImage);
            if (string.IsNullOrEmpty(imagePath))
            {
                imagePath = GetRelativePath(existingFiles.FirstOrDefault() ?? "") ?? "";
            }
        }

        userProfileImages.OtherImages = UpdateOtherImages(userProfileImages.OtherImages, GetRelativePathsArray(existingFiles));
        imagesRepository.Update(userProfileImages);
        await unitOfWork.CommitAsync();

        IEnumerable<string> others = userProfileImages.OtherImages.Split(',');
        return new UserImageUploadResponse(imagePath, others);
    }

    private string UpdateOtherImages(string otherImages, string[] existingFiles)
    {
        var otherImagesList = otherImages.Split(',').ToList();
        foreach (var file in existingFiles)
        {
            if (!otherImagesList.Contains(file))
            {
                otherImagesList.Add(file);
            }
        }
        return string.Join(",", otherImagesList);
    }

    private string GetRelativePath(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
        {
            return "";
        }
        var frontendIndex = imagePath.IndexOf("frontend");
        if (frontendIndex == -1)
        {
            return imagePath;
        }
        return imagePath.Substring(frontendIndex + "frontend".Length + 1);
    }

    private IEnumerable<string> GetRelativePaths(IEnumerable<string> paths)
    {
        return paths.Select(GetRelativePath);
    }
    private string[] GetRelativePathsArray(IEnumerable<string> paths)
    {
        return paths.Select(GetRelativePath).ToArray();
    }
}
