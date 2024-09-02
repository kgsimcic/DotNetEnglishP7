using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Dot.Net.WebApi.Services;
using Moq;

namespace Dot.Net.WebApi.Tests
{
    public class CurvePointServiceTests
    {
        private readonly Mock<IRepository<CurvePoint>> _mockRepository;
        public CurvePointService? CurvePointService;
        public CurvePoint[] mockCurvePoints;

        public CurvePointServiceTests()
        {
            _mockRepository = new Mock<IRepository<CurvePoint>>();
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
        }

        // Test Get All method

        [Fact]
        public async Task GetAll_Nonempty_ShouldReturnArray()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(mockCurvePoints);
            CurvePointService = new CurvePointService(_mockRepository.Object);

            // Act
            var getAllResult = await CurvePointService.GetAllCurvePoints();

            // Assert
            Assert.NotEmpty(getAllResult);
            Assert.IsType<CurvePoint[]>(getAllResult);
        }

        [Fact]
        public async Task GetAll_Empty_ShouldReturnEmptyArray()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(Array.Empty<CurvePoint>());
            CurvePointService = new CurvePointService(_mockRepository.Object);

            // Act
            var getAllResult = await CurvePointService.GetAllCurvePoints();

            // Assert
            Assert.Empty(getAllResult);
            Assert.IsType<CurvePoint[]>(getAllResult);
        }

        // Test GetCurvePointByName method

        [Fact]
        public async Task GetCurvePointByName_ShouldReturnCurvePoint()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(mockCurvePoints[0]);
            CurvePointService = new CurvePointService(_mockRepository.Object);

            // Act
            var getResult = await CurvePointService.GetCurvePoint(1);

            // Assert
            Assert.Equal(1, getResult.CurveId);
            Assert.IsType<CurvePoint>(getResult);
        }

        [Fact]
        public async Task GetCurvePointById_NotFound_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(4)).ReturnsAsync((CurvePoint)null!);
            CurvePointService = new CurvePointService(_mockRepository.Object);

            // Act
            var getResult = await CurvePointService.GetCurvePoint(4);

            // Assert
            Assert.Null(getResult);
        }

        // Test Create CurvePoint method

        [Fact]
        public async Task CreateCurvePoint_Valid_ShouldReturnSuccess()
        {
            var newCurvePoint = new CurvePoint
            {
                Id = 1,
                CurveId = 1,
                AsOfDate = DateTime.Now,
                Term = 1.0m,
                Value = 1.0m
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(newCurvePoint.Id)).ReturnsAsync((CurvePoint)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            CurvePointService = new CurvePointService(_mockRepository.Object);

            // Act
            var result = await CurvePointService.CreateCurvePoint(newCurvePoint);

            // Assert
            Assert.IsType<Result>(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateCurvePoint_NotFound_ShouldReturnException()
        {
            var newCurvePoint = new CurvePoint
            {
                Id = 1,
                CurveId = 1,
                AsOfDate = DateTime.Now,
                Term = 1.0m,
                Value = 1.0m
            };

            // Arrange
            _mockRepository.Setup(repo => repo.GetById(newCurvePoint.Id)).ReturnsAsync((CurvePoint)null!);
            CurvePointService = new CurvePointService(_mockRepository.Object);

            // Act and Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await CurvePointService.UpdateCurvePoint(newCurvePoint.Id, newCurvePoint));
        }

        [Fact]
        public async Task UpdateCurvePoint_Valid_ShouldReturnSuccess()
        {
            var existingCurvePoint = mockCurvePoints[0];
            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(existingCurvePoint);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            CurvePointService = new CurvePointService(_mockRepository.Object);

            // Act
            var result = await CurvePointService.UpdateCurvePoint(1, existingCurvePoint);

            // Assert
            Assert.IsType<Result>(result);
            Assert.True(result.IsSuccess);
        }

        // Test DeleteCurvePoint method
        [Fact]
        public async Task DeleteCurvePoint_NotFound_ShouldReturnException()
        {

            // Arrange
            _mockRepository.Setup(repo => repo.GetById(3)).ReturnsAsync((CurvePoint)null!);
            CurvePointService = new CurvePointService(_mockRepository.Object);

            // Act and assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await CurvePointService.DeleteCurvePoint(3));
        }

        [Fact]
        public async Task DeleteCurvePoint_Valid_ShouldReturnSuccess()
        {
            var existingCurvePoint = mockCurvePoints[0];
            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(existingCurvePoint);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            CurvePointService = new CurvePointService(_mockRepository.Object);

            // Act
            var result = await CurvePointService.DeleteCurvePoint(1);

            // Assert
            Assert.IsType<int>(result);
            Assert.Equal(1, result);
        }

        // Test validation: 4 for create method, 1 for update method

        [Fact]
        public async Task CreateCurvePoint_Invalid_ShouldReturnCurveIdNegOrZeroError()
        {
            var newCurvePoint = new CurvePoint
            {
                Id = 1,
                CurveId = 0,
                AsOfDate = DateTime.Now,
                Term = 1.0m,
                Value = 1.0m
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(newCurvePoint.Id)).ReturnsAsync((CurvePoint)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            CurvePointService = new CurvePointService(_mockRepository.Object);

            // Act
            var result = await CurvePointService.CreateCurvePoint(newCurvePoint);

            // Assert - should give buy quantity negative error
            Assert.IsType<Result>(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("CurvePoint.CurveIdNegOrZero", result.Error.Code);
        }

        [Fact]
        public async Task CreateCurvePoint_Invalid_ShouldReturnValueNegativeError()
        {
            var newCurvePoint = new CurvePoint
            {
                Id = 1,
                CurveId = 1,
                AsOfDate = DateTime.Now,
                Term = 1.0m,
                Value = -1.0m
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(newCurvePoint.Id)).ReturnsAsync((CurvePoint)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            CurvePointService = new CurvePointService(_mockRepository.Object);

            // Act
            var result = await CurvePointService.CreateCurvePoint(newCurvePoint);

            // Assert - should give buy quantity negative error
            Assert.IsType<Result>(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("CurvePoint.ValueNegative", result.Error.Code);
        }

        [Fact]
        public async Task CreateCurvePoint_Invalid_ShouldReturnTermNegativeError()
        {
            var newCurvePoint = new CurvePoint
            {
                Id = 1,
                CurveId = 1,
                AsOfDate = DateTime.Now,
                Term = -1.0m,
                Value = 1.0m
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(newCurvePoint.Id)).ReturnsAsync((CurvePoint)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            CurvePointService = new CurvePointService(_mockRepository.Object);

            // Act
            var result = await CurvePointService.CreateCurvePoint(newCurvePoint);

            // Assert - should give buy quantity negative error
            Assert.IsType<Result>(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("CurvePoint.TermNegative", result.Error.Code);
        }
    }
}