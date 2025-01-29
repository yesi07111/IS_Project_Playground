using AutoFixture;
using Playground.Domain.Entities;
using Playground.Infraestructure.Services;

namespace Playground.Tests.Services
{
    public class ImageServiceUnitTests
    {
        private readonly Fixture _fixture;
        private readonly ImageService _imageService;

        public ImageServiceUnitTests()
        {
            _fixture = new Fixture();

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _imageService = new ImageService();
        }

        [Fact]
        public void ImageService_GetImageForActivity_ReturnsImagePath()
        {
            // Arrange
            var activity = _fixture.Build<Activity>()
                .With(a => a.Facility, _fixture.Build<Facility>()
                    .With(f => f.Type, "Taller")
                    .Create())
                .Create();

            // Act
            var imagePath = _imageService.GetImageForActivity(activity);

            // Assert
            Assert.NotNull(imagePath);
            Assert.Contains("images/", imagePath);
        }

        [Fact]
        public async Task ImageService_DownloadAndSaveUserProfileImageAsync_ThrowsExceptionOnInvalidUrl()
        {
            // Arrange
            var imageUrl = "https://invalid-url.com/image.jpg";
            var username = "user1";

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _imageService.DownloadAndSaveUserProfileImageAsync(imageUrl, username));
        }
    }
}