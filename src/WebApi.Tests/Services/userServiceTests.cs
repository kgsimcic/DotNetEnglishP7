
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Dot.Net.WebApi.Repositories;
using Moq;
using Xunit;
using Microsoft.Identity.Client;
using Dot.Net.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Dot.Net.WebApi.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IRepository<User>> _mockRepository;
        public UserService? userService;
        public User[] mockUsers;

        public UserServiceTests()
        {
            _mockRepository = new Mock<IRepository<User>>();
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
        }

        // Test Get All method

        [Fact]
        public void GetAll_Nonempty_ShouldReturnArray()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAll()).Returns(mockUsers);
            userService = new UserService(_mockRepository.Object);

            // Act
            var getAllResult = userService.GetAllUsers();

            // Assert
            Assert.NotEmpty(getAllResult);
            Assert.IsType<User[]>(getAllResult);
        }

        [Fact]
        public void GetAll_Empty_ShouldReturnEmptyArray()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAll()).Returns(Array.Empty<User>());
            userService = new UserService(_mockRepository.Object);

            // Act
            var getAllResult = userService.GetAllUsers();

            // Assert
            Assert.Empty(getAllResult);
            Assert.IsType<User[]>(getAllResult);
        }

        // Test GetUserByName method

/*        [Fact]
        public async Task GetUserByName_ShouldReturnUser()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.FindByUserName("Admin")).ReturnsAsync(mockUsers[0]);
            userService = new UserService(_mockRepository.Object);

            // Act
            var getResult = await userService.GetUserByName("Admin");

            // Assert
            Assert.Equal("Admin", getResult.UserName);
            Assert.IsType<User>(getResult);
        }*/

/*        [Fact]
        public async Task GetUserByName_NotFound_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.FindByUserName("NotFound")).ReturnsAsync((User)null!);
            userService = new UserService(_mockRepository.Object);

            // Act
            var getResult = await userService.GetUserByName("NotFound");

            // Assert
            Assert.Null(getResult);
            Assert.IsType<User>(getResult);
        }*/

        // Test Create User method

        [Fact]
        public async Task CreateUser_Valid_ShouldReturnSuccess()
        {
            var newUser = new User
            {
                Id = 3,
                UserName = "NewUser",
                Password = "Password123@"
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(3)).Returns((User)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            userService = new UserService(_mockRepository.Object);

            // Act
            var result = await userService.CreateUser(newUser);

            // Assert
            Assert.IsType<int>(result);
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task UpdateUser_NotFound_ShouldReturnException()
        {
            var newUser = new User
            {
                Id = 3,
                UserName = "NewUser",
                Password = "pwd"
            };

            // Arrange
            _mockRepository.Setup(repo => repo.GetById(3)).Returns((User)null!);
            userService = new UserService(_mockRepository.Object);

            // Act and Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await userService.UpdateUser(3, newUser));
        }

        [Fact]
        public async Task UpdateUser_Valid_ShouldReturnSuccess()
        {
            var existingUser = mockUsers[0];
            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(1)).Returns(existingUser);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            userService = new UserService(_mockRepository.Object);

            // Act
            var result = await userService.UpdateUser(1, existingUser);

            // Assert
            Assert.IsType<int>(result);
            Assert.Equal(1, result);
        }

        // Test DeleteUser method
        [Fact]
        public async Task DeleteUser_NotFound_ShouldReturnException()
        {

            // Arrange
            _mockRepository.Setup(repo => repo.GetById(3)).Returns((User)null!);
            userService = new UserService(_mockRepository.Object);

            // Act and assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await userService.DeleteUser(3));
        }

        [Fact]
        public async Task DeleteUser_Valid_ShouldReturnSuccess()
        {
            var existingUser = mockUsers[0];
            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(1)).Returns(existingUser);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            userService = new UserService(_mockRepository.Object);

            // Act
            var result = await userService.DeleteUser(1);

            // Assert
            Assert.IsType<int>(result);
            Assert.Equal(1, result);
        }

        // test validation: 3 for create method, one for update method
        // to make sure it is called.

    }
}