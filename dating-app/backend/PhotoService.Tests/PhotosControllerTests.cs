using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PhotoService.Controllers;
using PhotoService.Models;
using Xunit;

namespace PhotoService.Tests
{
    public class PhotosControllerTests
    {
        [Fact]
        public async Task UploadPhoto_ValidFile_ReturnsCreatedResult()
        {
            var controller = new PhotosController();
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.jpg");
            mockFile.Setup(f => f.Length).Returns(1024);
            mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default))
                   .Returns(Task.CompletedTask);

            var request = new UploadPhotoRequest
            {
                UserId = "user123",
                File = mockFile.Object,
                IsMain = true
            };

            var result = await controller.UploadPhoto(request);

            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task UploadPhoto_NoFile_ReturnsBadRequest()
        {
            var controller = new PhotosController();
            var request = new UploadPhotoRequest
            {
                UserId = "user123",
                File = null!
            };

            var result = await controller.UploadPhoto(request);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void GetUserPhotos_ValidUserId_ReturnsPhotos()
        {
            var controller = new PhotosController();
            var userId = "user123";

            var result = controller.GetUserPhotos(userId);

            Assert.IsType<List<Photo>>(result.Value);
        }
    }
}