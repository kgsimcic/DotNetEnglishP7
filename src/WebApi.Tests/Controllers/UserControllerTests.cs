using System;
using Dot.Net.WebApi.Controllers;
using System.Linq;
using WebApi.Services;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System.Collections.Generic;
using Dot.Net.WebApi.Domain;

namespace Dot.Net.WebApi.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockService;
        public UserController controller;
        public User[] nonemptyUsers;
        public User[] emptyUsers;

        public UserControllerTests()
        {
            _mockService = new Mock<IUserService>();
            nonemptyUsers = new User[]
            {
                new User
                {
                    Id = 1,
                    UserName = "Admin",
                    Password = "Admin_pw",
                    FullName = "A",
                    Role = "admin user"
                },
                new User
                {
                    Id = 2,
                    UserName = "Test",
                    Password = "Test",
                    FullName = "Test",
                    Role = "test user"
                }
            };
        }

        [Fact]
        public void GetAllUsers_ShouldReturnOkResult()
        {
            // Arrange
            _mockService.Setup(service => service.GetAllUsers()).Returns(nonemptyUsers);
            controller = new UserController(_mockService.Object);

            // Act
            var result = controller.Home();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultUsers = Assert.IsType<List<User>>(okResult.Value);
            Assert.Equal(2, resultUsers.Count);
        }

        [Fact]
        public void GetUser_ShouldReturnOkResult()
        {
            // Arrange
            _mockService.Setup(service => service.GetUserByName("Admin")).Returns(nonemptyUsers.Where);
            controller = new UserController(_mockService.Object);

            // Act
            var result = controller.Home();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultUsers = Assert.IsType<List<User>>(okResult.Value);
            Assert.Equal(2, resultUsers.Count);
        }

    }
}