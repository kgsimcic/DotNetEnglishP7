using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dot.Net.WebApi.Tests
{
    public class TradeControllerTests
    {
        private readonly Mock<ITradeService> _mockService;
        public TradeController? controller;
        public Trade[] mockTrades;
        public Mock<ILogger<TradeController>> mockLogger;

        public TradeControllerTests()
        {
            _mockService = new Mock<ITradeService>();
            mockTrades = new Trade[]
            {
                new()
                {
                    TradeId = 1,
                    Account = "Test",
                    BuyQuantity = 1.0m,
                    SellQuantity = .5m,
                    BuyPrice = 20.75m,
                    SellPrice = 1.34m
                },
                new()
                {
                    TradeId = 2,
                    Account = "Test2",
                    BuyQuantity = .75m,
                    SellQuantity = .2m,
                    BuyPrice = 21.77m,
                    SellPrice = 11.49m
                }
            };
            mockLogger = new Mock<ILogger<TradeController>>();
        }

        // Test Get All Method

        [Fact]
        public async Task GetAllTrades_Nonempty_ShouldReturnOk()
        {

            // Arrange
            _mockService.Setup(service => service.GetAllTrades()).ReturnsAsync(mockTrades);
            controller = new TradeController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetAllTrades();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultTrades = Assert.IsType<Trade[]>(okResult.Value);
            Assert.Equal(2, resultTrades.Count());
            Assert.Equal(1.0m, mockTrades[0].BuyQuantity);
        }

        [Fact]
        public async Task GetAllTrades_Empty_ShouldReturnEmpty()
        {

            // Arrange
            _mockService.Setup(service => service.GetAllTrades()).ReturnsAsync(Array.Empty<Trade>());
            controller = new TradeController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetAllTrades();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultTrades = Assert.IsType<Trade[]>(okResult.Value);
            Assert.Empty(resultTrades);
        }

        [Fact]
        public async Task CreateTrade_Null_ShouldReturnBadRequest()
        {
            // Arrange -- no setup required because controller handles this logic and does not call the service
            controller = new TradeController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateTrade((Trade)null!);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreateTrade_AlreadyExists_ShouldReturnConflict()
        {

            // Arrange
            var existingTrade = mockTrades.First();
            _mockService.Setup(service => service.GetTrade(existingTrade.TradeId)).ReturnsAsync(existingTrade);
            controller = new TradeController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateTrade(existingTrade);

            // Assert
            var conflictObjectResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(409, conflictObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreateTrade_Invalid_ShouldReturnValidationError()
        {
            // Arrange
            Trade invalidTrade = new()
            {
                TradeId = 3,
                Account = "TestAgain",
                BuyQuantity = -500,
                SellQuantity = -.1m,
                BuyPrice = 11.11m,
                SellPrice = 1.11m
            };

            _mockService.Setup(service => service.GetTrade(invalidTrade.TradeId)).ReturnsAsync((Trade)null!);
            _mockService.Setup(service => service.CreateTrade(invalidTrade)).ReturnsAsync(Result.Failure(
                    new Error("trade.BuyQuantityNegative", "Trade Buy Quantity cannot be negative.")));
            controller = new TradeController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateTrade(invalidTrade);

            // Assert
            var updatedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, updatedResult.StatusCode);

        }

        [Fact]
        public async Task CreateTrade_Valid_ShouldReturnCreated()
        {
            // Arrange
            Trade newTrade = new()
            {
                TradeId = 3,
                Account = "TestAgain",
                BuyQuantity = 1.1m,
                SellQuantity = .1m,
                BuyPrice = 11.11m,
                SellPrice = 1.11m
            };
            _mockService.Setup(service => service.GetTrade(newTrade.TradeId)).ReturnsAsync((Trade)null!);
            _mockService.Setup(service => service.CreateTrade(newTrade)).ReturnsAsync(Result.Success);
            controller = new TradeController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateTrade(newTrade);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task UpdateTrade_NullInput_ShouldReturnBadRequest()
        {
            // Arrange
            Trade updateTrade = (Trade)null!;
            controller = new TradeController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateTrade(1, updateTrade);

            // Assert
            var createdResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, createdResult.StatusCode);
        }

        [Fact]
        public async Task UpdateTrade_NotFound_ShouldReturnNotFound()
        {
            // Arrange
            Trade updateTrade = new()
            {
                TradeId = 3,
                Account = "TestAgain",
                BuyQuantity = 1.1m,
                SellQuantity = .1m,
                BuyPrice = 11.11m,
                SellPrice = 1.11m
            };
            _mockService.Setup(service => service.GetTrade(updateTrade.TradeId)).ReturnsAsync((Trade)null!);
            _mockService.Setup(service => service.UpdateTrade(3,updateTrade)).ThrowsAsync(new KeyNotFoundException());
            controller = new TradeController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateTrade(3, updateTrade);

            // Assert
            var createdResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, createdResult.StatusCode);
        }

        [Fact]
        public async Task UpdateTrade_IdMismatch_ShouldReturnBadRequest()
        {
            Trade updateTrade = new()
            {
                TradeId = 3,
                Account = "TestAgain",
                BuyQuantity = 1.1m,
                SellQuantity = .1m,
                BuyPrice = 11.11m,
                SellPrice = 1.11m
            };

            // Arrange -- no setup required because controller handles this logic and does not call the service
            controller = new TradeController(_mockService.Object, mockLogger.Object);

            // Act - gave value of 1 for id because it does not matter - regardless of id value, method will produce error
            // if updated user is null
            var result = await controller.UpdateTrade(1, updateTrade);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);

        }

        [Fact]
        public async Task UpdateTrade_Invalid_ShouldReturnValidationError()
        {
            // Arrange
            Trade invalidTrade = new()
            {
                TradeId = 3,
                Account = "TestAgain",
                BuyQuantity = -500,
                SellQuantity = -.1m,
                BuyPrice = 11.11m,
                SellPrice = 1.11m
            };

            _mockService.Setup(service => service.GetTrade(invalidTrade.TradeId)).ReturnsAsync(invalidTrade);
            _mockService.Setup(service => service.UpdateTrade(3, invalidTrade)).ReturnsAsync(Result.Failure(
                    new Error("trade.BuyQuantityNegative", "Trade Buy Quantity cannot be negative.")));
            controller = new TradeController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateTrade(3, invalidTrade);

            // Assert
            var updatedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, updatedResult.StatusCode);

        }

        [Fact]
        public async Task UpdateTrade_Valid_ShouldReturnSuccess()
        {
            // Arrange
            Trade updateTrade = new()
            {
                TradeId = 3,
                Account = "TestAgain",
                BuyQuantity = 1.1m,
                SellQuantity = .1m,
                BuyPrice = 11.11m,
                SellPrice = 1.11m
            };
            _mockService.Setup(service => service.GetTrade(updateTrade.TradeId)).ReturnsAsync(updateTrade);
            _mockService.Setup(service => service.UpdateTrade(3, updateTrade)).ReturnsAsync(Result.Success);
            controller = new TradeController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateTrade(3, updateTrade);

            // Assert
            var updatedResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, updatedResult.StatusCode);
        }

        [Fact]
        public async Task DeleteTrade_NotFound_ShouldReturnNotFound()
        {
            // Arrange

            _mockService.Setup(service => service.DeleteTrade(3)).ThrowsAsync(new KeyNotFoundException());
            controller = new TradeController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteTrade(3);

            // Assert
            var deletedResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, deletedResult.StatusCode);
        }

        [Fact]
        public async Task DeleteTrade_Valid_ShouldReturnSuccess()
        {
            // Arrange
            Trade deleteTrade = new()
            {
                TradeId = 3,
                Account = "TestAgain",
                BuyQuantity = 1.1m,
                SellQuantity = .1m,
                BuyPrice = 11.11m,
                SellPrice = 1.11m
            };

            _mockService.Setup(service => service.GetTrade(deleteTrade.TradeId)).ReturnsAsync(deleteTrade);
            _mockService.Setup(service => service.DeleteTrade(3)).ReturnsAsync(1);
            controller = new TradeController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteTrade(3);

            // Assert
            var deletedResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, deletedResult.StatusCode);
        }

    }
}