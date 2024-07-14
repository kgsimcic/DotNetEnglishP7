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
        public void GetAllTrades_Nonempty_ShouldReturnOk()
        {

            // Arrange
            _mockService.Setup(service => service.GetAllTrades()).Returns(mockTrades);
            controller = new TradeController(_mockService.Object, mockLogger.Object);

            // Act
            var result = controller.GetAllTrades();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultTrades = Assert.IsType<List<Trade>>(okResult.Value);
            Assert.Equal(2, resultTrades.Count);
            Assert.Equal(1.0m, mockTrades[0].BuyQuantity);
        }

        [Fact]
        public void GetAllTrades_Empty_ShouldReturnEmpty()
        {

            // Arrange
            _mockService.Setup(service => service.GetAllTrades()).Returns(mockTrades);
            controller = new TradeController(_mockService.Object, mockLogger.Object);

            // Act
            var result = controller.GetAllTrades();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultTrades = Assert.IsType<List<Trade>>(okResult.Value);
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
            _mockService.Setup(service => service.GetTrade(existingTrade.TradeId)).Returns(existingTrade);
            controller = new TradeController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateTrade(existingTrade);

            // Assert
            var conflictObjectResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(409, conflictObjectResult.StatusCode);
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
            _mockService.Setup(service => service.GetTrade(newTrade.TradeId)).Returns((Trade)null!);
            _mockService.Setup(service => service.CreateTrade(newTrade)).ReturnsAsync(Result.Success);
            controller = new TradeController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateTrade(newTrade);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }




    }
}