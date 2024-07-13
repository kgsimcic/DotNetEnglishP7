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
        public UserController? controller;
        public User[] mockUsers;
        public Mock<ILogger<UserController>> mockLogger;

        public UserControllerTests()
        {
            _mockService = new Mock<IUserService>();
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

        // Test Home Method

        [Fact]
        public void Home_Nonempty_ShouldReturnOk()
        {

            // Arrange
            _mockService.Setup(service => service.GetAllUsers()).Returns(mockUsers);
            controller = new UserController(_mockService.Object, mockLogger.Object);

            // Act
            var result = controller.Home();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultUsers = Assert.IsType<List<User>>(okResult.Value);
            Assert.Equal(2, resultUsers.Count);
        }

        [Fact]
        public void Home_Empty_ShouldReturnEmpty()
        {

            // Arrange
            _mockService.Setup(service => service.GetAllUsers()).Returns(Array.Empty<User>());
            controller = new UserController(_mockService.Object, mockLogger.Object);

            // Act
            var result = controller.Home();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultUsers = Assert.IsType<List<User>>(okResult.Value);
            Assert.Empty(resultUsers);
        }

        // Test GetUserByUserName method

        // [Fact]
/*        public async Task GetUserByUserName_Admin_ShouldReturnOk()
        {
            // Arrange
            _mockService.Setup(service => service.GetUserByName("Admin")).ReturnsAsync(mockUsers[0]);
            controller = new UserController(_mockService.Object);

            // Act
            var result = await controller.GetUserByUserName("Admin");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultUser = Assert.IsType<User>(okResult.Value);
            Assert.Equal(1, resultUser.Id);

        }*/

        /*[Fact]
        public async Task GetUserByUserName_NotFound_ShouldReturnNotFound()
        {
            // Arrange
            _mockService.Setup(service => service.GetUserByName("ValueNotInData")).ReturnsAsync((User)null!);
            controller = new UserController(_mockService.Object);

            // Act
            var result = await controller.GetUserByUserName("ValueNotInData");

            // Assert
            var notFoundResult =  Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);

        }*/

        // Test CreateUser method

        [Fact]
        public async Task CreateUser_Null_ShouldReturnBadRequest()
        {
            // Arrange -- no setup required because controller handles this logic and does not call the service
            controller = new UserController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateUser(null);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreateUser_AlreadyExists_ShouldReturnConflict()
        {

            // Arrange
            var existingUser = mockUsers.First();
            _mockService.Setup(service => service.GetUserById(existingUser.Id)).Returns(existingUser);
            controller = new UserController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateUser(existingUser);

            // Assert
            var conflictObjectResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(409, conflictObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreateUser_Valid_ShouldReturnCreated()
        {
            // Arrange
            User newUser = new()
            {
                Id = 3,
                UserName = "NewUser",
                Password = "pwd"
            };
            _mockService.Setup(service => service.GetUserById(newUser.Id)).Returns((User)null!);
            _mockService.Setup(service => service.CreateUser(newUser)).ReturnsAsync(Result.Success);
            controller = new UserController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateUser(newUser);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }

        // Test Update User method

        [Fact]
        public async Task UpdateUser_Null_ShouldReturnBadRequest()
        {

            // Arrange -- no setup required because controller handles this logic and does not call the service
            controller = new UserController(_mockService.Object, mockLogger.Object);

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

            // Arrange -- no setup required because controller handles this logic and does not call the service
            controller = new UserController(_mockService.Object, mockLogger.Object);

            // Act - gave value of 1 for id because it does not matter - regardless of id value, method will produce error
            // if updated user is null
            var result = await controller.UpdateUser(1, updateUser);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);

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
            _mockService.Setup(service => service.UpdateUser(updateUser.Id, updateUser)).ThrowsAsync(new KeyNotFoundException());
            controller = new UserController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateUser(updateUser.Id, updateUser);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
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
                Password = "pwd"
            };
            _mockService.Setup(service => service.UpdateUser(updateUser.Id, updateUser)).ReturnsAsync(Result.Success);
            controller = new UserController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateUser(updateUser.Id, updateUser);

            // Assert
            var updatedResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, updatedResult.StatusCode);
        }

        // Test DeleteUser method

        [Fact]
        public async Task DeleteUser_NotFound_ShouldReturnNotFound()
        {

            // Arrange
            _mockService.Setup(service => service.DeleteUser(3)).ThrowsAsync(new KeyNotFoundException());
            controller = new UserController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteUser(3);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);

        }

        [Fact]
        public async Task DeleteUser_Valid_ShouldReturnNoContent()
        {
            // Arrange
            _mockService.Setup(service => service.DeleteUser(1)).ReturnsAsync(1);
            controller = new UserController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteUser(1);

            // Assert
            var updatedResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, updatedResult.StatusCode);

        }

















    }
}