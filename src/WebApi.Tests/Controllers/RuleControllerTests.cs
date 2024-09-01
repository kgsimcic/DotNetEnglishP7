using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace Dot.Net.WebApi.Tests
{
    public class RuleControllerTests
    {
        private readonly Mock<IRuleService> _mockService;
        public RuleController? controller;
        public Rule[] mockRules;
        public Mock<ILogger<RuleController>> mockLogger;

        public RuleControllerTests()
        {
            _mockService = new Mock<IRuleService>();
            mockRules = new Rule[]
            {
                new()
                {
                    Id = 1,
                    Name = "Test",
                    Description = "test",
                    Json = "{'object': 'value'}",
                    Template = null,
                    SqlStr = null,
                    SqlPart = null
                },
                new()
                {
                    Id = 2,
                    Name = "Test2",
                    Description = "test2",
                    Json = "{'object': 'value'}",
                    Template = "hi",
                    SqlStr = null,
                    SqlPart = null
                }
            };
            mockLogger = new Mock<ILogger<RuleController>>();
        }

        // Test Get All Method

        [Fact]
        public async Task GetAllRules_Nonempty_ShouldReturnOk()
        {

            // Arrange
            _mockService.Setup(service => service.GetAllRules()).ReturnsAsync(mockRules);
            controller = new RuleController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetAllRules();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultRules = Assert.IsType<Rule[]>(okResult.Value);
            Assert.Equal(2, resultRules.Count());
            Assert.Equal("Test", mockRules[0].Name);
        }

        [Fact]
        public async Task GetAllRules_Empty_ShouldReturnEmpty()
        {

            // Arrange
            _mockService.Setup(service => service.GetAllRules()).ReturnsAsync(Array.Empty<Rule>());
            controller = new RuleController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetAllRules();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultRules = Assert.IsType<Rule[]>(okResult.Value);
            Assert.Empty(resultRules);
        }

        [Fact]
        public async Task CreateRule_Null_ShouldReturnBadRequest()
        {
            // Arrange -- no setup required because controller handles this logic and does not call the service
            controller = new RuleController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateRule((Rule)null!);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreateRule_AlreadyExists_ShouldReturnConflict()
        {

            // Arrange
            var existingRule = mockRules.First();
            _mockService.Setup(service => service.GetRule(existingRule.Id)).ReturnsAsync(existingRule);
            controller = new RuleController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateRule(existingRule);

            // Assert
            var conflictObjectResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(409, conflictObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreateRule_Invalid_ShouldReturnValidationError()
        {
            // Arrange
            Rule invalidRule = new()
            {
                Id = 1,
                Name = null,
                Description = "test",
                Json = "{'object': 'value'}",
                Template = null,
                SqlStr = null,
                SqlPart = null
            };

            _mockService.Setup(service => service.GetRule(invalidRule.Id)).ReturnsAsync((Rule)null!);
            _mockService.Setup(service => service.CreateRule(invalidRule)).ReturnsAsync(Result.Failure(
                    new Error("Rule.NameRequired", "Rule Name is required.")));
            controller = new RuleController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateRule(invalidRule);

            // Assert
            var updatedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, updatedResult.StatusCode);

        }

        [Fact]
        public async Task CreateRule_Valid_ShouldReturnCreated()
        {
            // Arrange
            Rule newRule = new()
            {
                Id = 1,
                Name = "Test",
                Description = "test",
                Json = "{'object': 'value'}",
                Template = null,
                SqlStr = null,
                SqlPart = null
            };
            _mockService.Setup(service => service.GetRule(newRule.Id)).ReturnsAsync((Rule)null!);
            _mockService.Setup(service => service.CreateRule(newRule)).ReturnsAsync(Result.Success);
            controller = new RuleController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateRule(newRule);
            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task UpdateRule_NullInput_ShouldReturnBadRequest()
        {
            // Arrange
            Rule updateRule = (Rule)null!;
            controller = new RuleController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateRule(1, updateRule);

            // Assert
            var createdResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, createdResult.StatusCode);
        }

        [Fact]
        public async Task UpdateRule_NotFound_ShouldReturnNotFound()
        {
            // Arrange
            Rule updateRule = new()
            {
                Id = 1,
                Name = "Test",
                Description = "test",
                Json = "{'object': 'value'}",
                Template = null,
                SqlStr = null,
                SqlPart = null
            };
            _mockService.Setup(service => service.GetRule(updateRule.Id)).ReturnsAsync((Rule)null!);
            _mockService.Setup(service => service.UpdateRule(updateRule.Id, updateRule)).ThrowsAsync(new KeyNotFoundException());
            controller = new RuleController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateRule(updateRule.Id, updateRule);

            // Assert
            var createdResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, createdResult.StatusCode);
        }

        [Fact]
        public async Task UpdateRule_IdMismatch_ShouldReturnBadRequest()
        {
            Rule updateRule = new()
            {
                Id = 1,
                Name = "Test",
                Description = "test",
                Json = "{'object': 'value'}",
                Template = null,
                SqlStr = null,
                SqlPart = null
            };

            // Arrange -- no setup required because controller handles this logic and does not call the service
            controller = new RuleController(_mockService.Object, mockLogger.Object);

            // Act - gave value of 1 for id because it does not matter - regardless of id value, method will produce error
            // if updated user is null
            var result = await controller.UpdateRule(2, updateRule);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);

        }

        [Fact]
        public async Task UpdateRule_Invalid_ShouldReturnValidationError()
        {
            // Arrange
            Rule invalidRule = new()
            {
                Id = 1,
                Name = null,
                Description = "test",
                Json = "{'object': 'value'}",
                Template = null,
                SqlStr = null,
                SqlPart = null
            };

            _mockService.Setup(service => service.GetRule(invalidRule.Id)).ReturnsAsync(invalidRule);
            _mockService.Setup(service => service.UpdateRule(invalidRule.Id, invalidRule)).ReturnsAsync(Result.Failure(
                    new Error("Rule.NameRequired", "Rule Name is required.")));
            controller = new RuleController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateRule(invalidRule.Id, invalidRule);

            // Assert
            var updatedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, updatedResult.StatusCode);

        }

        [Fact]
        public async Task UpdateRule_Valid_ShouldReturnSuccess()
        {
            // Arrange
            Rule updateRule = new()
            {
                Id = 1,
                Name = "Test",
                Description = "test",
                Json = "{'object': 'value'}",
                Template = null,
                SqlStr = null,
                SqlPart = null
            };
            _mockService.Setup(service => service.GetRule(updateRule.Id)).ReturnsAsync(updateRule);
            _mockService.Setup(service => service.UpdateRule(updateRule.Id, updateRule)).ReturnsAsync(Result.Success);
            controller = new RuleController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateRule(updateRule.Id, updateRule);

            // Assert
            var updatedResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, updatedResult.StatusCode);
        }

        [Fact]
        public async Task DeleteRule_NotFound_ShouldReturnNotFound()
        {
            // Arrange

            _mockService.Setup(service => service.DeleteRule(3)).ThrowsAsync(new KeyNotFoundException());
            controller = new RuleController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteRule(3);

            // Assert
            var deletedResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, deletedResult.StatusCode);
        }

        [Fact]
        public async Task DeleteRule_Valid_ShouldReturnSuccess()
        {
            // Arrange
            Rule deleteRule = new()
            {
                Id = 1,
                Name = "Test",
                Description = "test",
                Json = "{'object': 'value'}",
                Template = null,
                SqlStr = null,
                SqlPart = null
            };

            _mockService.Setup(service => service.GetRule(deleteRule.Id)).ReturnsAsync(deleteRule);
            _mockService.Setup(service => service.DeleteRule(deleteRule.Id)).ReturnsAsync(1);
            controller = new RuleController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteRule(deleteRule.Id);

            // Assert
            var deletedResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, deletedResult.StatusCode);
        }
    }
}