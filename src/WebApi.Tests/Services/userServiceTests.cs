
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Dot.Net.WebApi.Repositories;
using Moq;
using Xunit;
using Microsoft.Identity.Client;
using Dot.Net.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;

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
                    Password = "Admin_pw5",
                    FullName = "A",
                    Role = "admin user"
                },
                new()
                {
                    Id = 2,
                    UserName = "Test",
                    Password = "TestPassw0rd!",
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
            Assert.IsType<Result>(result);
            Assert.True(result.IsSuccess);
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
            Assert.IsType<Result>(result);
            Assert.True(result.IsSuccess);
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

        // Test validation: 4 for create method, 1 for update method

        [Fact]
        public async Task CreateUser_Invalid_ShouldReturnNeedsSpecialError()
        {
            var newUser = new User
            {
                Id = 3,
                UserName = "NewUser",
                Password = "Password1"
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(3)).Returns((User)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            userService = new UserService(_mockRepository.Object);

            // Act
            var result = await userService.CreateUser(newUser);

            // Assert - should give needs special char error
            Assert.IsType<Result>(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("User.PasswordNeedsSpecial", result.Error.Code);
        }

        [Fact]
        public async Task CreateUser_Invalid_ShouldReturnNeedsUppercaseError()
        {
            var newUser = new User
            {
                Id = 3,
                UserName = "NewUser",
                Password = "password1"
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(3)).Returns((User)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            userService = new UserService(_mockRepository.Object);

            // Act
            var result = await userService.CreateUser(newUser);

            // Assert - should give needs uppercase error
            Assert.IsType<Result>(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("User.PasswordNeedsUppercase", result.Error.Code);
        }

        [Fact]
        public async Task CreateUser_Invalid_ShouldReturnUsernameRequiredError()
        {
            var newUser = new User
            {
                Id = 3,
                UserName = "",
                Password = "Password123@"
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(3)).Returns((User)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            userService = new UserService(_mockRepository.Object);

            // Act
            var result = await userService.CreateUser(newUser);

            // Assert - should give error username required
            Assert.IsType<Result>(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("User.UsernameRequired", result.Error.Code);
        }

        [Fact]
        public async Task CreateUser_Invalid_ShouldReturnPasswordNeedsNumberError()
        {
            var newUser = new User
            {
                Id = 3,
                UserName = "Hi",
                Password = "Password@"
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(3)).Returns((User)null!);
            userService = new UserService(_mockRepository.Object);

            // Act
            var result = await userService.CreateUser(newUser);

            // Assert - should give error password needs number
            Assert.IsType<Result>(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("User.PasswordNeedsNumber", result.Error.Code);
        }

        [Fact]
        public async Task UpdateUser_Invalid_ShouldReturnPasswordTooShortError()
        {
            var newUser = new User
            {
                Id = 3,
                UserName = "NewUser",
                Password = "pass"
            };

            var existingUser = mockUsers[0];
            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(1)).Returns(existingUser);
            userService = new UserService(_mockRepository.Object);

            // Act
            var result = await userService.UpdateUser(1, newUser);

            // Assert - should give password too short error
            Assert.IsType<Result>(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("User.PasswordTooShort", result.Error.Code);
        }

    }
}