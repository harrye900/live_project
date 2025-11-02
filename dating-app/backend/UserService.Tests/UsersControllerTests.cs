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
        public void CreateUser_InvalidEmail_ReturnsBadRequest()
        {
            var controller = new UsersController();
            var request = new CreateUserRequest
            {
                Email = "invalid-email",
                Password = "password123",
                Name = "Test User"
            };

            var result = controller.CreateUser(request);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void CreateUser_ShortPassword_ReturnsBadRequest()
        {
            var controller = new UsersController();
            var request = new CreateUserRequest
            {
                Email = "test@example.com",
                Password = "123",
                Name = "Test User"
            };

            var result = controller.CreateUser(request);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void CreateUser_EmptyName_ReturnsBadRequest()
        {
            var controller = new UsersController();
            var request = new CreateUserRequest
            {
                Email = "test@example.com",
                Password = "password123",
                Name = ""
            };

            var result = controller.CreateUser(request);

            Assert.IsType<BadRequestObjectResult>(result.Result);
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

        [Fact]
        public void Login_EmptyEmail_ReturnsBadRequest()
        {
            var controller = new UsersController();
            var loginRequest = new LoginRequest
            {
                Email = "",
                Password = "password123"
            };

            var result = controller.Login(loginRequest);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetUser_ValidId_ReturnsUser()
        {
            var controller = new UsersController();
            var createResult = controller.CreateUser(new CreateUserRequest
            {
                Email = "test@example.com",
                Password = "password123",
                Name = "Test User"
            });
            var createdUser = ((CreatedAtActionResult)createResult.Result).Value as User;

            var result = controller.GetUser(createdUser.Id);

            Assert.IsType<User>(result.Value);
        }

        [Fact]
        public void GetUser_InvalidId_ReturnsNotFound()
        {
            var controller = new UsersController();

            var result = controller.GetUser("invalid-id");

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetUsers_ReturnsAllUsers()
        {
            var controller = new UsersController();

            var result = controller.GetUsers();

            Assert.NotNull(result.Value);
        }

        [Fact]
        public void AddPhoto_ValidData_ReturnsOk()
        {
            var controller = new UsersController();
            var createResult = controller.CreateUser(new CreateUserRequest
            {
                Email = "test@example.com",
                Password = "password123",
                Name = "Test User"
            });
            var createdUser = ((CreatedAtActionResult)createResult.Result).Value as User;

            var result = controller.AddPhoto(createdUser.Id, "http://example.com/photo.jpg");

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void AddPhoto_EmptyUserId_ReturnsBadRequest()
        {
            var controller = new UsersController();

            var result = controller.AddPhoto("", "http://example.com/photo.jpg");

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddPhoto_EmptyPhotoUrl_ReturnsBadRequest()
        {
            var controller = new UsersController();

            var result = controller.AddPhoto("user-id", "");

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}