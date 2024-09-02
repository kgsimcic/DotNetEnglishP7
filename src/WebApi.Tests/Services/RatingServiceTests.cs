using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Dot.Net.WebApi.Services;

namespace Dot.net.WebApi.Tests
{
    public class RatingServiceTests
    {
        private readonly Mock<IRepository<Rating>> _mockRepository;
        public RatingService? RatingService;
        public Rating[] mockRatings;

        public RatingServiceTests()
        {
            _mockRepository = new Mock<IRepository<Rating>>();
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
        }

        // Test Get All method

        [Fact]
        public async Task GetAll_Nonempty_ShouldReturnArray()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(mockRatings);
            RatingService = new RatingService(_mockRepository.Object);

            // Act
            var getAllResult = await RatingService.GetAllRatings();

            // Assert
            Assert.NotEmpty(getAllResult);
            Assert.IsType<Rating[]>(getAllResult);
        }

        [Fact]
        public async Task GetAll_Empty_ShouldReturnEmptyArray()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(Array.Empty<Rating>());
            RatingService = new RatingService(_mockRepository.Object);

            // Act
            var getAllResult = await RatingService.GetAllRatings();

            // Assert
            Assert.Empty(getAllResult);
            Assert.IsType<Rating[]>(getAllResult);
        }

        // Test GetRatingByName method

        [Fact]
        public async Task GetRatingByName_ShouldReturnRating()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(mockRatings[0]);
            RatingService = new RatingService(_mockRepository.Object);

            // Act
            var getResult = await RatingService.GetRating(1);

            // Assert
            Assert.Equal("Test", getResult.MoodysRating);
            Assert.IsType<Rating>(getResult);
        }

        [Fact]
        public async Task GetRatingById_NotFound_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(4)).ReturnsAsync((Rating)null!);
            RatingService = new RatingService(_mockRepository.Object);

            // Act
            var getResult = await RatingService.GetRating(4);

            // Assert
            Assert.Null(getResult);
        }

        // Test Create Rating method

        [Fact]
        public async Task CreateRating_Valid_ShouldReturnSuccess()
        {
            var newRating = new Rating
            {
                Id = 1,
                MoodysRating = "Test",
                SandPRating = null,
                FitchRating = null,
                OrderNumber = 2
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(newRating.Id)).ReturnsAsync((Rating)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            RatingService = new RatingService(_mockRepository.Object);

            // Act
            var result = await RatingService.CreateRating(newRating);

            // Assert
            Assert.IsType<Result>(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateRating_NotFound_ShouldReturnException()
        {
            var newRating = new Rating
            {
                Id = 1,
                MoodysRating = "Test",
                SandPRating = null,
                FitchRating = null,
                OrderNumber = 2
            };

            // Arrange
            _mockRepository.Setup(repo => repo.GetById(newRating.Id)).ReturnsAsync((Rating)null!);
            RatingService = new RatingService(_mockRepository.Object);

            // Act and Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await RatingService.UpdateRating(newRating.Id, newRating));
        }

        [Fact]
        public async Task UpdateRating_Valid_ShouldReturnSuccess()
        {
            var existingRating = mockRatings[0];
            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(existingRating);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            RatingService = new RatingService(_mockRepository.Object);

            // Act
            var result = await RatingService.UpdateRating(1, existingRating);

            // Assert
            Assert.IsType<Result>(result);
            Assert.True(result.IsSuccess);
        }

        // Test DeleteRating method
        [Fact]
        public async Task DeleteRating_NotFound_ShouldReturnException()
        {

            // Arrange
            _mockRepository.Setup(repo => repo.GetById(3)).ReturnsAsync((Rating)null!);
            RatingService = new RatingService(_mockRepository.Object);

            // Act and assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await RatingService.DeleteRating(3));
        }

        [Fact]
        public async Task DeleteRating_Valid_ShouldReturnSuccess()
        {
            var existingRating = mockRatings[0];
            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(existingRating);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            RatingService = new RatingService(_mockRepository.Object);

            // Act
            var result = await RatingService.DeleteRating(1);

            // Assert
            Assert.IsType<int>(result);
            Assert.Equal(1, result);
        }

        // Test validation: 4 for create method, 1 for update method

        [Fact]
        public async Task CreateRating_Invalid_ShouldReturnBuyQuantityNegativeError()
        {
            var newRating = new Rating
            {
                Id = 1,
                MoodysRating = null,
                SandPRating = null,
                FitchRating = null,
                OrderNumber = -2
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(newRating.Id)).ReturnsAsync((Rating)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            RatingService = new RatingService(_mockRepository.Object);

            // Act
            var result = await RatingService.CreateRating(newRating);

            // Assert - should give buy quantity negative error
            Assert.IsType<Result>(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Rating.OrderNumberNegative", result.Error.Code);
        }
    }
}