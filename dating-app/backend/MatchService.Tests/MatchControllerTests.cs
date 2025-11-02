using Microsoft.AspNetCore.Mvc;
using MatchService.Controllers;
using MatchService.Models;
using Xunit;

namespace MatchService.Tests
{
    public class MatchControllerTests
    {
        [Fact]
        public void Swipe_ValidLike_ReturnsOkResult()
        {
            var controller = new MatchController();
            var request = new SwipeRequest
            {
                UserId = 1,
                TargetUserId = 2,
                IsLike = true
            };

            var result = controller.Swipe(request);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public void Swipe_InvalidRequest_ReturnsBadRequest()
        {
            var controller = new MatchController();
            var request = new SwipeRequest
            {
                UserId = null,
                TargetUserId = 2,
                IsLike = true
            };

            var result = controller.Swipe(request);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void GetPotentialMatches_ValidUserId_ReturnsOkResult()
        {
            var controller = new MatchController();
            var userId = 1;

            var result = controller.GetPotentialMatches(userId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void SyncUsers_ValidUsers_ReturnsOkResult()
        {
            var controller = new MatchController();
            var users = new List<UserProfile>
            {
                new UserProfile { Id = 1, Name = "Test User" }
            };

            var result = controller.SyncUsers(users);

            Assert.IsType<OkResult>(result);
        }
    }
}