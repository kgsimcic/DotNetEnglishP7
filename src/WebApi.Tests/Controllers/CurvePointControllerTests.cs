using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dot.Net.WebApi.Tests
{
    public class CurvePointControllerTests
    {
        private readonly Mock<ICurvePointService> _mockService;
        public CurvePointController? controller;
        public CurvePoint[] mockCurvePoints;
        public Mock<ILogger<CurvePointController>> mockLogger;

        public CurvePointControllerTests()
        {
            _mockService = new Mock<ICurvePointService>();
            mockCurvePoints = new CurvePoint[]
            {
                new()
                {
                    Id = 1,
                    CurveId = 1,
                    AsOfDate = DateTime.Now,
                    Term = 1.0m,
                    Value = 1.0m
                },
                new()
                {
                    Id = 2,
                    CurveId = 2,
                    AsOfDate = DateTime.Now,
                    Term = 2.0m,
                    Value = 2.0m
                }
            };
            mockLogger = new Mock<ILogger<CurvePointController>>();
        }

        // Test Get All Method

        [Fact]
        public async Task GetAllCurvePoints_Nonempty_ShouldReturnOk()
        {

            // Arrange
            _mockService.Setup(service => service.GetAllCurvePoints()).ReturnsAsync(mockCurvePoints);
            controller = new CurvePointController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetAllCurvePoints();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultCurvePoints = Assert.IsType<CurvePoint[]>(okResult.Value);
            Assert.Equal(2, resultCurvePoints.Count());
            Assert.Equal(1, mockCurvePoints[0].CurveId);
        }

        [Fact]
        public async Task GetAllCurvePoints_Empty_ShouldReturnEmpty()
        {

            // Arrange
            _mockService.Setup(service => service.GetAllCurvePoints()).ReturnsAsync(Array.Empty<CurvePoint>());
            controller = new CurvePointController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetAllCurvePoints();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultCurvePoints = Assert.IsType<CurvePoint[]>(okResult.Value);
            Assert.Empty(resultCurvePoints);
        }

        [Fact]
        public async Task CreateCurvePoint_Null_ShouldReturnBadRequest()
        {
            // Arrange -- no setup required because controller handles this logic and does not call the service
            controller = new CurvePointController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateCurvePoint((CurvePoint)null!);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreateCurvePoint_AlreadyExists_ShouldReturnConflict()
        {

            // Arrange
            var existingCurvePoint = mockCurvePoints.First();
            _mockService.Setup(service => service.GetCurvePoint(existingCurvePoint.Id)).ReturnsAsync(existingCurvePoint);
            controller = new CurvePointController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateCurvePoint(existingCurvePoint);

            // Assert
            var conflictObjectResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(409, conflictObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreateCurvePoint_Invalid_ShouldReturnValidationError()
        {
            // Arrange
            CurvePoint invalidCurvePoint = new()
            {
                Id = 1,
                CurveId = 1,
                AsOfDate = DateTime.Now,
                Term = 1.0m,
                Value = 1.0m
            };

            _mockService.Setup(service => service.GetCurvePoint(invalidCurvePoint.Id)).ReturnsAsync((CurvePoint)null!);
            _mockService.Setup(service => service.CreateCurvePoint(invalidCurvePoint)).ReturnsAsync(Result.Failure(
                    new Error("CurvePoint.NameRequired", "CurvePoint Name is required.")));
            controller = new CurvePointController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateCurvePoint(invalidCurvePoint);

            // Assert
            var updatedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, updatedResult.StatusCode);

        }

        [Fact]
        public async Task CreateCurvePoint_Valid_ShouldReturnCreated()
        {
            // Arrange
            CurvePoint newCurvePoint = new()
            {
                Id = 1,
                CurveId = 1,
                AsOfDate = DateTime.Now,
                Term = 1.0m,
                Value = 1.0m
            };
            _mockService.Setup(service => service.GetCurvePoint(newCurvePoint.Id)).ReturnsAsync((CurvePoint)null!);
            _mockService.Setup(service => service.CreateCurvePoint(newCurvePoint)).ReturnsAsync(Result.Success);
            controller = new CurvePointController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateCurvePoint(newCurvePoint);
            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task UpdateCurvePoint_NullInput_ShouldReturnBadRequest()
        {
            // Arrange
            CurvePoint updateCurvePoint = (CurvePoint)null!;
            controller = new CurvePointController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateCurvePoint(1, updateCurvePoint);

            // Assert
            var createdResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, createdResult.StatusCode);
        }

        [Fact]
        public async Task UpdateCurvePoint_NotFound_ShouldReturnNotFound()
        {
            // Arrange
            CurvePoint updateCurvePoint = new()
            {
                Id = 1,
                CurveId = 1,
                AsOfDate = DateTime.Now,
                Term = 1.0m,
                Value = 1.0m
            };
            _mockService.Setup(service => service.GetCurvePoint(updateCurvePoint.Id)).ReturnsAsync((CurvePoint)null!);
            _mockService.Setup(service => service.UpdateCurvePoint(updateCurvePoint.Id, updateCurvePoint)).ThrowsAsync(new KeyNotFoundException());
            controller = new CurvePointController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateCurvePoint(updateCurvePoint.Id, updateCurvePoint);

            // Assert
            var createdResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, createdResult.StatusCode);
        }

        [Fact]
        public async Task UpdateCurvePoint_IdMismatch_ShouldReturnBadRequest()
        {
            CurvePoint updateCurvePoint = new()
            {
                Id = 1,
                CurveId = 1,
                AsOfDate = DateTime.Now,
                Term = 1.0m,
                Value = 1.0m
            };

            // Arrange -- no setup required because controller handles this logic and does not call the service
            controller = new CurvePointController(_mockService.Object, mockLogger.Object);

            // Act - gave value of 1 for id because it does not matter - regardless of id value, method will produce error
            // if updated user is null
            var result = await controller.UpdateCurvePoint(2, updateCurvePoint);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);

        }

        [Fact]
        public async Task UpdateCurvePoint_Invalid_ShouldReturnValidationError()
        {
            // Arrange
            CurvePoint invalidCurvePoint = new()
            {
                Id = 1,
                CurveId = 1,
                AsOfDate = DateTime.Now,
                Term = -1.0m,
                Value = 1.0m
            };

            _mockService.Setup(service => service.GetCurvePoint(invalidCurvePoint.Id)).ReturnsAsync(invalidCurvePoint);
            _mockService.Setup(service => service.UpdateCurvePoint(invalidCurvePoint.Id, invalidCurvePoint)).ReturnsAsync(Result.Failure(
                    new Error("CurvePoint.NameRequired", "CurvePoint Name is required.")));
            controller = new CurvePointController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateCurvePoint(invalidCurvePoint.Id, invalidCurvePoint);

            // Assert
            var updatedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, updatedResult.StatusCode);

        }

        [Fact]
        public async Task UpdateCurvePoint_Valid_ShouldReturnSuccess()
        {
            // Arrange
            CurvePoint updateCurvePoint = new()
            {
                Id = 1,
                CurveId = 1,
                AsOfDate = DateTime.Now,
                Term = 1.0m,
                Value = 1.0m
            };
            _mockService.Setup(service => service.GetCurvePoint(updateCurvePoint.Id)).ReturnsAsync(updateCurvePoint);
            _mockService.Setup(service => service.UpdateCurvePoint(updateCurvePoint.Id, updateCurvePoint)).ReturnsAsync(Result.Success);
            controller = new CurvePointController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateCurvePoint(updateCurvePoint.Id, updateCurvePoint);

            // Assert
            var updatedResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, updatedResult.StatusCode);
        }

        [Fact]
        public async Task DeleteCurvePoint_NotFound_ShouldReturnNotFound()
        {
            // Arrange

            _mockService.Setup(service => service.DeleteCurvePoint(3)).ThrowsAsync(new KeyNotFoundException());
            controller = new CurvePointController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteCurvePoint(3);

            // Assert
            var deletedResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, deletedResult.StatusCode);
        }

        [Fact]
        public async Task DeleteCurvePoint_Valid_ShouldReturnSuccess()
        {
            // Arrange
            CurvePoint deleteCurvePoint = new()
            {
                Id = 1,
                CurveId = 1,
                AsOfDate = DateTime.Now,
                Term = 1.0m,
                Value = 1.0m
            };

            _mockService.Setup(service => service.GetCurvePoint(deleteCurvePoint.Id)).ReturnsAsync(deleteCurvePoint);
            _mockService.Setup(service => service.DeleteCurvePoint(deleteCurvePoint.Id)).ReturnsAsync(1);
            controller = new CurvePointController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteCurvePoint(deleteCurvePoint.Id);

            // Assert
            var deletedResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, deletedResult.StatusCode);
        }
    }
}