using System;
using Dot.Net.WebApi.Controllers;
using System.Linq;
using Dot.Net.WebApi.Services;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System.Collections.Generic;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace Dot.Net.WebApi.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockService;
        private readonly Mock<ITokenService> _tokenService;
        public UserController? controller;
        public User[] mockUsers;
        public Mock<ILogger<UserController>> mockLogger;

        public UserControllerTests()
        {
            _mockService = new Mock<IUserService>();
            _tokenService = new Mock<ITokenService>();
            mockUsers = new User[]
            {
                new()
                {
                    Id = 1,
                    UserName = "Admin",
                    Password = "Admin_pw",
                    FullName = "A",
                    Role = "admin user"
                },
                new()
                {
                    Id = 2,
                    UserName = "Test",
                    Password = "Test",
                    FullName = "Test",
                    Role = "test user"
                }
            };
            mockLogger = new Mock<ILogger<UserController>>();
        }

        // Test GetAllusers Method

        [Fact]
        public async Task GetAllUsers_Nonempty_ShouldReturnOk()
        {

            // Arrange
            _mockService.Setup(service => service.GetAllUsers()).ReturnsAsync(mockUsers);
            controller = new UserController(_mockService.Object, _tokenService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultUsers = Assert.IsType<PartialUser[]>(okResult.Value);
            Assert.Equal(2, resultUsers.Count());
        }

        [Fact]
        public async Task GetAllUsers_Empty_ShouldReturnEmpty()
        {

            // Arrange
            _mockService.Setup(service => service.GetAllUsers()).ReturnsAsync(Array.Empty<User>());
            controller = new UserController(_mockService.Object, _tokenService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultUsers = Assert.IsType<PartialUser[]>(okResult.Value);
            Assert.Empty(resultUsers);
        }

        // Test GetUserByUserName method

        [Fact]
        public async Task GetUserByUserName_Admin_ShouldReturnOk()
        {
            // Arrange
            _mockService.Setup(service => service.GetUserByName("Admin")).ReturnsAsync(mockUsers[0]);
            controller = new UserController(_mockService.Object, _tokenService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetUserByUserName("Admin");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultUser = Assert.IsType<PartialUser>(okResult.Value);
            Assert.Equal(1, resultUser.Id);

        }

        [Fact]
        public async Task GetUserByUserName_NotFound_ShouldReturnNotFound()
        {
            // Arrange
            _mockService.Setup(service => service.GetUserByName("ValueNotInData")).ReturnsAsync((User)null!);
            controller = new UserController(_mockService.Object, _tokenService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetUserByUserName("ValueNotInData");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);

        }

        // Test CreateUser method

        [Fact]
        public async Task Register_Null_ShouldReturnBadRequest()
        {
            // Arrange -- no setup required because controller handles this logic and does not call the service
            controller = new UserController(_mockService.Object, _tokenService.Object, mockLogger.Object);

            // Act
            var result = await controller.Register(null);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task Register_AlreadyExists_ShouldReturnConflict()
        {

            // Arrange
            var existingUser = mockUsers.First();
            _mockService.Setup(service => service.GetUserById(existingUser.Id)).ReturnsAsync(existingUser);
            _mockService.Setup(service => service.GetUserByName(existingUser.UserName)).ReturnsAsync(existingUser);
            controller = new UserController(_mockService.Object, _tokenService.Object, mockLogger.Object);

            // Act
            var result = await controller.Register(existingUser);

            // Assert
            var conflictObjectResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(409, conflictObjectResult.StatusCode);
        }

        [Fact]
        public async Task Register_Valid_ShouldReturnCreated()
        {
            // Arrange
            User newUser = new()
            {
                Id = 3,
                UserName = "NewUser",
                Password = "Pwd@@44long"
            };
            _mockService.Setup(service => service.GetUserById(newUser.Id)).ReturnsAsync((User)null!);
            _mockService.Setup(service => service.CreateUser(newUser)).ReturnsAsync(Result.Success);
            controller = new UserController(_mockService.Object, _tokenService.Object, mockLogger.Object);

            // Act
            var result = await controller.Register(newUser);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }

        // Test Update User method

        [Fact]
        public async Task UpdateUser_Null_ShouldReturnBadRequest()
        {

            // Arrange -- no setup required because controller handles this logic and does not call the service
            controller = new UserController(_mockService.Object, _tokenService.Object, mockLogger.Object);

            // Act - gave value of 1 for id because it does not matter - regardless of id value, method will produce error
            // if updated user is null
            var result = await controller.UpdateUser(1, null);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);

        }

        [Fact]
        public async Task UpdateUser_IdMismatch_ShouldReturnBadRequest()
        {
            User updateUser = new()
            {
                Id = 3,
                UserName = "NewUser",
                Password = "pwd"
            };


            // Arrange
            _mockService.Setup(service => service.GetUserById(1)).ReturnsAsync(mockUsers[0]);
            controller = new UserController(_mockService.Object, _tokenService.Object, mockLogger.Object);

            // Act - gave value of 1 for id because it does not matter - regardless of id value, method will produce error
            // if updated user is null
            var result = await controller.UpdateUser(1, updateUser);

            // Assert
            var unauthorizedObjectResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedObjectResult.StatusCode);

        }

        [Fact]
        public async Task UpdateUser_NotFound_ShouldReturnNotFound()
        {
            User updateUser = new()
            {
                Id = 3,
                UserName = "NewUser",
                Password = "pwd"
            };

            // Arrange
            _tokenService.Setup(service => service.ValidateToken(It.IsAny<string>())).Returns((int?)null!);
            controller = new UserController(_mockService.Object, _tokenService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateUser(updateUser.Id, updateUser);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);

        }

        [Fact]
        public async Task UpdateUser_Valid_ShouldReturnNoContent()
        {
            // Arrange
            User updateUser = new()
            {
                Id = 1,
                UserName = "NewUser",
                Password = "Pwd@@44long"
            };


            _mockService.Setup(service => service.UpdateUser(updateUser)).ReturnsAsync(Result.Success);
            _tokenService.Setup(service => service.ValidateToken(It.IsAny<string>())).Returns(1);
            _mockService.Setup(service => service.GetUserById(1)).ReturnsAsync(mockUsers[0]);
            controller = new UserController(_mockService.Object, _tokenService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateUser(updateUser.Id, updateUser);

            // Assert
            var updatedResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, updatedResult.StatusCode);
        }

        // Test DeleteUser method

        [Fact]
        public async Task DeleteUser_NotFound_ShouldReturnUnauthorized()
        {

            // Arrange
            _tokenService.Setup(service => service.ValidateToken(It.IsAny<string>())).Returns((int?)null!);
            controller = new UserController(_mockService.Object, _tokenService.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteUser(3);

            // Assert
            var notFoundResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, notFoundResult.StatusCode);

        }

        [Fact]
        public async Task DeleteUser_Valid_ShouldReturnNoContent()
        {
            // Arrange
            _mockService.Setup(service => service.DeleteUser(1)).ReturnsAsync(1);
            _tokenService.Setup(service => service.ValidateToken(It.IsAny<string>())).Returns(1);
            controller = new UserController(_mockService.Object, _tokenService.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteUser(1);

            // Assert
            var updatedResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, updatedResult.StatusCode);

        }



















    }
}