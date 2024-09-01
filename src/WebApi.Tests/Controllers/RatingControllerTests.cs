using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Tests
{
    public class RatingControllerTests
    {
        private readonly Mock<IRatingService> _mockService;
        public RatingController? controller;
        public Rating[] mockRatings;
        public Mock<ILogger<RatingController>> mockLogger;

        public RatingControllerTests()
        {
            _mockService = new Mock<IRatingService>();
            mockRatings = new Rating[]
            {
                new ()
                {
                    Id = 1,
                    MoodysRating = "Test",
                    SandPRating = null,
                    FitchRating = null,
                    OrderNumber = 2
                },
                new()
                {
                    Id = 2,
                    MoodysRating = "Test2",
                    SandPRating = null,
                    FitchRating = null,
                    OrderNumber = 3
                }
            };
            mockLogger = new Mock<ILogger<RatingController>>();
        }

        // Test Get All Method

        [Fact]
        public async Task GetAllRatings_Nonempty_ShouldReturnOk()
        {

            // Arrange
            _mockService.Setup(service => service.GetAllRatings()).ReturnsAsync(mockRatings);
            controller = new RatingController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetAllRatings();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultRatings = Assert.IsType<Rating[]>(okResult.Value);
            Assert.Equal(2, resultRatings.Count());
            Assert.Equal("Test", mockRatings[0].MoodysRating);
        }

        [Fact]
        public async Task GetAllRatings_Empty_ShouldReturnEmpty()
        {

            // Arrange
            _mockService.Setup(service => service.GetAllRatings()).ReturnsAsync(Array.Empty<Rating>());
            controller = new RatingController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetAllRatings();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultRatings = Assert.IsType<Rating[]>(okResult.Value);
            Assert.Empty(resultRatings);
        }

        [Fact]
        public async Task CreateRating_Null_ShouldReturnBadRequest()
        {
            // Arrange -- no setup required because controller handles this logic and does not call the service
            controller = new RatingController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateRating((Rating)null!);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreateRating_AlreadyExists_ShouldReturnConflict()
        {

            // Arrange
            var existingRating = mockRatings.First();
            _mockService.Setup(service => service.GetRating(existingRating.Id)).ReturnsAsync(existingRating);
            controller = new RatingController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateRating(existingRating);

            // Assert
            var conflictObjectResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(409, conflictObjectResult.StatusCode);
        }

        /*[Fact]
        public async Task CreateRating_Invalid_ShouldReturnValidationError()
        {
            // what is the controllers reaction if I try to pass in null to the order number? Not sure.
            // Arrange
            Rating invalidRating = new()
            {
                Id = 1,
                MoodysRating = "Test",
                SandPRating = null,
                FitchRating = null,
                OrderNumber = 1
            };

            _mockService.Setup(service => service.GetRating(invalidRating.Id)).ReturnsAsync((Rating)null!);
            _mockService.Setup(service => service.CreateRating(invalidRating)).ReturnsAsync(Result.Failure(
                    new Error("Rating.NameRequired", "Rating Name is required.")));
            controller = new RatingController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateRating(invalidRating);

            // Assert
            var updatedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, updatedResult.StatusCode);

        }*/

        [Fact]
        public async Task CreateRating_Valid_ShouldReturnCreated()
        {
            // Arrange
            Rating newRating = new()
            {
                Id = 1,
                MoodysRating = "Test",
                SandPRating = null,
                FitchRating = null,
                OrderNumber = 2
            };
            _mockService.Setup(service => service.GetRating(newRating.Id)).ReturnsAsync((Rating)null!);
            _mockService.Setup(service => service.CreateRating(newRating)).ReturnsAsync(Result.Success);
            controller = new RatingController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateRating(newRating);
            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task UpdateRating_NullInput_ShouldReturnBadRequest()
        {
            // Arrange
            Rating updateRating = (Rating)null!;
            controller = new RatingController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateRating(1, updateRating);

            // Assert
            var createdResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, createdResult.StatusCode);
        }

        [Fact]
        public async Task UpdateRating_NotFound_ShouldReturnNotFound()
        {
            // Arrange
            Rating updateRating = new()
            {
                Id = 1,
                MoodysRating = "Test",
                SandPRating = null,
                FitchRating = null,
                OrderNumber = 2
            };
            _mockService.Setup(service => service.GetRating(updateRating.Id)).ReturnsAsync((Rating)null!);
            _mockService.Setup(service => service.UpdateRating(updateRating.Id, updateRating)).ThrowsAsync(new KeyNotFoundException());
            controller = new RatingController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateRating(updateRating.Id, updateRating);

            // Assert
            var createdResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, createdResult.StatusCode);
        }

        [Fact]
        public async Task UpdateRating_IdMismatch_ShouldReturnBadRequest()
        {
            Rating updateRating = new()
            {
                Id = 1,
                MoodysRating = "Test",
                SandPRating = null,
                FitchRating = null,
                OrderNumber = 2
            };

            // Arrange -- no setup required because controller handles this logic and does not call the service
            controller = new RatingController(_mockService.Object, mockLogger.Object);

            // Act - gave value of 1 for id because it does not matter - regardless of id value, method will produce error
            // if updated user is null
            var result = await controller.UpdateRating(2, updateRating);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);

        }

        /*[Fact]
        public async Task UpdateRating_Invalid_ShouldReturnValidationError()
        {
            // Arrange
            Rating invalidRating = new()
            {
                Id = 1,
                MoodysRating = "Test",
                SandPRating = null,
                FitchRating = null,
                OrderNumber = 2
            };

            _mockService.Setup(service => service.GetRating(invalidRating.Id)).ReturnsAsync(invalidRating);
            _mockService.Setup(service => service.UpdateRating(invalidRating.Id, invalidRating)).ReturnsAsync(Result.Failure(
                    new Error("Rating.NameRequired", "Rating Name is required.")));
            controller = new RatingController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateRating(invalidRating.Id, invalidRating);

            // Assert
            var updatedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, updatedResult.StatusCode);

        }*/

        [Fact]
        public async Task UpdateRating_Valid_ShouldReturnSuccess()
        {
            // Arrange
            Rating updateRating = new()
            {
                Id = 1,
                MoodysRating = "Test",
                SandPRating = null,
                FitchRating = null,
                OrderNumber = 2
            };
            _mockService.Setup(service => service.GetRating(updateRating.Id)).ReturnsAsync(updateRating);
            _mockService.Setup(service => service.UpdateRating(updateRating.Id, updateRating)).ReturnsAsync(Result.Success);
            controller = new RatingController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateRating(updateRating.Id, updateRating);

            // Assert
            var updatedResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, updatedResult.StatusCode);
        }

        [Fact]
        public async Task DeleteRating_NotFound_ShouldReturnNotFound()
        {
            // Arrange

            _mockService.Setup(service => service.DeleteRating(3)).ThrowsAsync(new KeyNotFoundException());
            controller = new RatingController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteRating(3);

            // Assert
            var deletedResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, deletedResult.StatusCode);
        }

        [Fact]
        public async Task DeleteRating_Valid_ShouldReturnSuccess()
        {
            // Arrange
            Rating deleteRating = new()
            {
                Id = 1,
                MoodysRating = "Test",
                SandPRating = null,
                FitchRating = null,
                OrderNumber = 2
            };

            _mockService.Setup(service => service.GetRating(deleteRating.Id)).ReturnsAsync(deleteRating);
            _mockService.Setup(service => service.DeleteRating(deleteRating.Id)).ReturnsAsync(1);
            controller = new RatingController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteRating(deleteRating.Id);

            // Assert
            var deletedResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, deletedResult.StatusCode);
        }
    }
}