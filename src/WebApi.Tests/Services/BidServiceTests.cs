using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Dot.Net.WebApi.Services;

namespace Dot.Net.WebApi.Tests
{
    public class BidServiceTests
    {
        private readonly Mock<IRepository<Bid>> _mockRepository;
        public BidService? BidService;
        public Bid[] mockBids;

        public BidServiceTests()
        {
            _mockRepository = new Mock<IRepository<Bid>>();
            mockBids = new Bid[]
            {
                new()
                {
                    BidListId = 1,
                    Account = "Test",
                    Type = "Test Type",
                    BidQuantity = 1,
                    AskQuantity = 1,
                    BidAmount = 12,
                    Ask = 1
                 },

                new()
                {
                    BidListId = 2,
                    Account = "Test2",
                    Type = "Test Type2",
                    BidQuantity = 1,
                    AskQuantity = 2,
                    BidAmount = 11,
                    Ask = 2
                }
            };
        }

        // Test Get All method

        [Fact]
        public async Task GetAll_Nonempty_ShouldReturnArray()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(mockBids);
            BidService = new BidService(_mockRepository.Object);

            // Act
            var getAllResult = await BidService.GetAllBids();

            // Assert
            Assert.NotEmpty(getAllResult);
            Assert.IsType<Bid[]>(getAllResult);
        }

        [Fact]
        public async Task GetAll_Empty_ShouldReturnEmptyArray()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(Array.Empty<Bid>());
            BidService = new BidService(_mockRepository.Object);

            // Act
            var getAllResult = await BidService.GetAllBids();

            // Assert
            Assert.Empty(getAllResult);
            Assert.IsType<Bid[]>(getAllResult);
        }

        // Test GetBidByName method

        [Fact]
        public async Task GetBidByName_ShouldReturnBid()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(mockBids[0]);
            BidService = new BidService(_mockRepository.Object);

            // Act
            var getResult = await BidService.GetBid(1);

            // Assert
            Assert.Equal(1, getResult.BidListId);
            Assert.IsType<Bid>(getResult);
        }

        [Fact]
        public async Task GetBidById_NotFound_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(4)).ReturnsAsync((Bid)null!);
            BidService = new BidService(_mockRepository.Object);

            // Act
            var getResult = await BidService.GetBid(4);

            // Assert
            Assert.Null(getResult);
        }

        // Test Create Bid method

        [Fact]
        public async Task CreateBid_Valid_ShouldReturnSuccess()
        {
            var newBid = new Bid
            {
                BidListId = 1,
                Account = "Test",
                Type = "Test Type",
                BidQuantity = 1,
                AskQuantity = 1,
                BidAmount = 12,
                Ask = 1
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(newBid.BidListId)).ReturnsAsync((Bid)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            BidService = new BidService(_mockRepository.Object);

            // Act
            var result = await BidService.CreateBid(newBid);

            // Assert
            Assert.IsType<Result>(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateBid_NotFound_ShouldReturnException()
        {
            var newBid = new Bid
            {
                BidListId = 1,
                Account = "Test",
                Type = "Test Type",
                BidQuantity = 1,
                AskQuantity = 1,
                BidAmount = 12,
                Ask = 1
            };

            // Arrange
            _mockRepository.Setup(repo => repo.GetById(newBid.BidListId)).ReturnsAsync((Bid)null!);
            BidService = new BidService(_mockRepository.Object);

            // Act and Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await BidService.UpdateBid(newBid.BidListId, newBid));
        }

        [Fact]
        public async Task UpdateBid_Valid_ShouldReturnSuccess()
        {
            var existingBid = mockBids[0];
            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(existingBid);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            BidService = new BidService(_mockRepository.Object);

            // Act
            var result = await BidService.UpdateBid(1, existingBid);

            // Assert
            Assert.IsType<Result>(result);
            Assert.True(result.IsSuccess);
        }

        // Test DeleteBid method
        [Fact]
        public async Task DeleteBid_NotFound_ShouldReturnException()
        {

            // Arrange
            _mockRepository.Setup(repo => repo.GetById(3)).ReturnsAsync((Bid)null!);
            BidService = new BidService(_mockRepository.Object);

            // Act and assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await BidService.DeleteBid(3));
        }

        [Fact]
        public async Task DeleteBid_Valid_ShouldReturnSuccess()
        {
            var existingBid = mockBids[0];
            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(existingBid);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            BidService = new BidService(_mockRepository.Object);

            // Act
            var result = await BidService.DeleteBid(1);

            // Assert
            Assert.IsType<int>(result);
            Assert.Equal(1, result);
        }

        // Test validation: 4 for create method, 1 for update method

        [Fact]
        public async Task CreateBid_Invalid_ShouldReturnBidQuantityNegativeError()
        {
            var newBid = new Bid
            {
                BidListId = 1,
                Account = "Test",
                Type = "Test Type",
                BidQuantity = -1,
                AskQuantity = 1,
                BidAmount = 12,
                Ask = 1
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(newBid.BidListId)).ReturnsAsync((Bid)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            BidService = new BidService(_mockRepository.Object);

            // Act
            var result = await BidService.CreateBid(newBid);

            // Assert - should give buy quantity negative error
            Assert.IsType<Result>(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Bid.BidQuantityNegative", result.Error.Code);
        }

        [Fact]
        public async Task CreateBid_Invalid_ShouldReturnAskQuantityNegativeError()
        {
            var newBid = new Bid
            {
                BidListId = 1,
                Account = "Test",
                Type = "Test Type",
                BidQuantity = 1,
                AskQuantity = -1,
                BidAmount = 12,
                Ask = 1
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(newBid.BidListId)).ReturnsAsync((Bid)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            BidService = new BidService(_mockRepository.Object);

            // Act
            var result = await BidService.CreateBid(newBid);

            // Assert - should give buy quantity negative error
            Assert.IsType<Result>(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Bid.AskQuantityNegative", result.Error.Code);
        }

        [Fact]
        public async Task CreateBid_Invalid_ShouldReturnBidAmountNegativeError()
        {
            var newBid = new Bid
            {
                BidListId = 1,
                Account = "Test",
                Type = "Test Type",
                BidQuantity = 1,
                AskQuantity = 1,
                BidAmount = -12,
                Ask = 1
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(newBid.BidListId)).ReturnsAsync((Bid)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            BidService = new BidService(_mockRepository.Object);

            // Act
            var result = await BidService.CreateBid(newBid);

            // Assert - should give buy quantity negative error
            Assert.IsType<Result>(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Bid.BidAmountNegative", result.Error.Code);
        }

        [Fact]
        public async Task CreateBid_Invalid_ShouldReturnAskNegativeError()
        {
            var newBid = new Bid
            {
                BidListId = 1,
                Account = "Test",
                Type = "Test Type",
                BidQuantity = 1,
                AskQuantity = 1,
                BidAmount = 12,
                Ask = -1
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(newBid.BidListId)).ReturnsAsync((Bid)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            BidService = new BidService(_mockRepository.Object);

            // Act
            var result = await BidService.CreateBid(newBid);

            // Assert - should give buy quantity negative error
            Assert.IsType<Result>(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Bid.AskNegative", result.Error.Code);
        }
    }
}