using Microsoft.AspNetCore.Mvc;
using UserService.Controllers;
using UserService.Models;
using Xunit;

namespace UserService.Tests
{
    public class UsersControllerTests
    {
        [Fact]
        public void CreateUser_ValidUser_ReturnsCreatedResult()
        {
            var controller = new UsersController();
            var request = new CreateUserRequest
            {
                Email = "test@example.com",
                Password = "password123",
                Name = "Test User",
                Age = 25
            };

            var result = controller.CreateUser(request);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var user = Assert.IsType<User>(createdResult.Value);
            Assert.Equal(request.Email, user.Email);
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsOkResult()
        {
            var controller = new UsersController();
            controller.CreateUser(new CreateUserRequest
            {
                Email = "test@example.com",
                Password = "password123",
                Name = "Test User"
            });

            var loginRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var result = controller.Login(loginRequest);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            var controller = new UsersController();
            var loginRequest = new LoginRequest
            {
                Email = "invalid@example.com",
                Password = "wrongpassword"
            };

            var result = controller.Login(loginRequest);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}