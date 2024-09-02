using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Dot.Net.WebApi.Services;

namespace Dot.Net.WebApi.Tests
{
    public class TradeServiceTests
    {
        private readonly Mock<IRepository<Trade>> _mockRepository;
        public TradeService? TradeService;
        public Trade[] mockTrades;

        public TradeServiceTests()
        {
            _mockRepository = new Mock<IRepository<Trade>>();
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
        }

        // Test Get All method

        [Fact]
        public async Task GetAll_Nonempty_ShouldReturnArray()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(mockTrades);
            TradeService = new TradeService(_mockRepository.Object);

            // Act
            var getAllResult = await TradeService.GetAllTrades();

            // Assert
            Assert.NotEmpty(getAllResult);
            Assert.IsType<Trade[]>(getAllResult);
        }

        [Fact]
        public async Task GetAll_Empty_ShouldReturnEmptyArray()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(Array.Empty<Trade>());
            TradeService = new TradeService(_mockRepository.Object);

            // Act
            var getAllResult = await TradeService.GetAllTrades();

            // Assert
            Assert.Empty(getAllResult);
            Assert.IsType<Trade[]>(getAllResult);
        }

        // Test GetTradeByName method

        [Fact]
        public async Task GetTradeByName_ShouldReturnTrade()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(mockTrades[0]);
            TradeService = new TradeService(_mockRepository.Object);

            // Act
            var getResult = await TradeService.GetTrade(1);

            // Assert
            Assert.Equal("Test", getResult.Account);
            Assert.IsType<Trade>(getResult);
        }

        [Fact]
        public async Task GetTradeByName_NotFound_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(4)).ReturnsAsync((Trade)null!);
            TradeService = new TradeService(_mockRepository.Object);

            // Act
            var getResult = await TradeService.GetTrade(4);

            // Assert
            Assert.Null(getResult);
        }

        // Test Create Trade method

        [Fact]
        public async Task CreateTrade_Valid_ShouldReturnSuccess()
        {
            var newTrade = new Trade
            {
                TradeId = 1,
                Account = "Test",
                BuyQuantity = 1.0m,
                SellQuantity = .5m,
                BuyPrice = 20.75m,
                SellPrice = 1.34m
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(newTrade.TradeId)).ReturnsAsync((Trade)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            TradeService = new TradeService(_mockRepository.Object);

            // Act
            var result = await TradeService.CreateTrade(newTrade);

            // Assert
            Assert.IsType<Result>(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateTrade_NotFound_ShouldReturnException()
        {
            var newTrade = new Trade
            {
                TradeId = 1,
                Account = "Test",
                BuyQuantity = 1.0m,
                SellQuantity = .5m,
                BuyPrice = 20.75m,
                SellPrice = 1.34m
            };

            // Arrange
            _mockRepository.Setup(repo => repo.GetById(newTrade.TradeId)).ReturnsAsync((Trade)null!);
            TradeService = new TradeService(_mockRepository.Object);

            // Act and Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await TradeService.UpdateTrade(newTrade.TradeId, newTrade));
        }

        [Fact]
        public async Task UpdateTrade_Valid_ShouldReturnSuccess()
        {
            var existingTrade = mockTrades[0];
            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(existingTrade);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            TradeService = new TradeService(_mockRepository.Object);

            // Act
            var result = await TradeService.UpdateTrade(1, existingTrade);

            // Assert
            Assert.IsType<Result>(result);
            Assert.True(result.IsSuccess);
        }

        // Test DeleteTrade method
        [Fact]
        public async Task DeleteTrade_NotFound_ShouldReturnException()
        {

            // Arrange
            _mockRepository.Setup(repo => repo.GetById(3)).ReturnsAsync((Trade)null!);
            TradeService = new TradeService(_mockRepository.Object);

            // Act and assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await TradeService.DeleteTrade(3));
        }

        [Fact]
        public async Task DeleteTrade_Valid_ShouldReturnSuccess()
        {
            var existingTrade = mockTrades[0];
            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(existingTrade);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            TradeService = new TradeService(_mockRepository.Object);

            // Act
            var result = await TradeService.DeleteTrade(1);

            // Assert
            Assert.IsType<int>(result);
            Assert.Equal(1, result);
        }

        // Test validation: 4 for create method, 1 for update method

        [Fact]
        public async Task CreateTrade_Invalid_ShouldReturnBuyQuantityNegativeError()
        {
            var newTrade = new Trade
            {
                TradeId = 1,
                Account = "Test",
                BuyQuantity = -1.2m,
                SellQuantity = .5m,
                BuyPrice = 20.75m,
                SellPrice = 1.34m
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(newTrade.TradeId)).ReturnsAsync((Trade)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            TradeService = new TradeService(_mockRepository.Object);

            // Act
            var result = await TradeService.CreateTrade(newTrade);

            // Assert - should give buy quantity negative error
            Assert.IsType<Result>(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Trade.BuyQuantityNegative", result.Error.Code);
        }

        [Fact]
        public async Task CreateTrade_Invalid_ShouldReturnBuyPriceNegativeError()
        {
            var newTrade = new Trade
            {
                TradeId = 1,
                Account = "Test",
                BuyQuantity = 1.2m,
                SellQuantity = .5m,
                BuyPrice = -20.75m,
                SellPrice = 1.34m
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(newTrade.TradeId)).ReturnsAsync((Trade)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            TradeService = new TradeService(_mockRepository.Object);

            // Act
            var result = await TradeService.CreateTrade(newTrade);

            // Assert
            Assert.IsType<Result>(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Trade.BuyPriceNegative", result.Error.Code);
        }

        [Fact]
        public async Task CreateTrade_Invalid_ShouldReturnSellQuantityNegativeError()
        {
            var newTrade = new Trade
            {
                TradeId = 1,
                Account = "Test",
                BuyQuantity = 1.2m,
                SellQuantity = -.5m,
                BuyPrice = 20.75m,
                SellPrice = 1.34m
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(newTrade.TradeId)).ReturnsAsync((Trade)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            TradeService = new TradeService(_mockRepository.Object);

            // Act
            var result = await TradeService.CreateTrade(newTrade);

            // Assert
            Assert.IsType<Result>(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Trade.SellQuantityNegative", result.Error.Code);
        }

        [Fact]
        public async Task CreateTrade_Invalid_ShouldReturnSellPriceNegative()
        {
            var newTrade = new Trade
            {
                TradeId = 1,
                Account = "Test",
                BuyQuantity = 1.2m,
                SellQuantity = .5m,
                BuyPrice = 20.75m,
                SellPrice = -1.34m
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(newTrade.TradeId)).ReturnsAsync((Trade)null!);
            TradeService = new TradeService(_mockRepository.Object);

            // Act
            var result = await TradeService.CreateTrade(newTrade);

            // Assert
            Assert.IsType<Result>(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Trade.SellPriceNegative", result.Error.Code);
        }
    }
}