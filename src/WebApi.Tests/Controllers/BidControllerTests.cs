using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Tests
{
    public class BidControllerTests
    {
        private readonly Mock<IBidService> _mockService;
        public BidController? controller;
        public Bid[] mockBids;
        public Mock<ILogger<BidController>> mockLogger;

        public BidControllerTests()
        {
            _mockService = new Mock<IBidService>();
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
            mockLogger = new Mock<ILogger<BidController>>();
        }

        // Test Get All Method

        [Fact]
        public async Task GetAllBids_Nonempty_ShouldReturnOk()
        {

            // Arrange
            _mockService.Setup(service => service.GetAllBids()).ReturnsAsync(mockBids);
            controller = new BidController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetAllBids();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultBids = Assert.IsType<Bid[]>(okResult.Value);
            Assert.Equal(2, resultBids.Count());
            Assert.Equal("Test", mockBids[0].Account);
        }

        [Fact]
        public async Task GetAllBids_Empty_ShouldReturnEmpty()
        {

            // Arrange
            _mockService.Setup(service => service.GetAllBids()).ReturnsAsync(Array.Empty<Bid>());
            controller = new BidController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetAllBids();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultBids = Assert.IsType<Bid[]>(okResult.Value);
            Assert.Empty(resultBids);
        }

        [Fact]
        public async Task CreateBid_Null_ShouldReturnBadRequest()
        {
            // Arrange -- no setup required because controller handles this logic and does not call the service
            controller = new BidController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateBid((Bid)null!);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreateBid_AlreadyExists_ShouldReturnConflict()
        {

            // Arrange
            var existingBid = mockBids.First();
            _mockService.Setup(service => service.GetBid(existingBid.BidListId)).ReturnsAsync(existingBid);
            controller = new BidController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateBid(existingBid);

            // Assert
            var conflictObjectResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(409, conflictObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreateBid_Invalid_ShouldReturnValidationError()
        {
            // Arrange
            Bid invalidBid = new()
            {
                BidListId = 1,
                Account = "Test",
                Type = "Test Type",
                BidQuantity = -1,
                AskQuantity = 1,
                BidAmount = 12,
                Ask = 1
            };

            _mockService.Setup(service => service.GetBid(invalidBid.BidListId)).ReturnsAsync((Bid)null!);
            _mockService.Setup(service => service.CreateBid(invalidBid)).ReturnsAsync(Result.Failure(
                    new Error("bid.BidQuantityNegative", "Bid Quantity cannot be negative.")));
            controller = new BidController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateBid(invalidBid);

            // Assert
            var updatedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, updatedResult.StatusCode);

        }

        [Fact]
        public async Task CreateBid_Valid_ShouldReturnCreated()
        {
            // Arrange
            Bid newBid = new()
            {
                BidListId = 1,
                Account = "Test",
                Type = "Test Type",
                BidQuantity = 1,
                AskQuantity = 1,
                BidAmount = 12,
                Ask = 1
            };
            _mockService.Setup(service => service.GetBid(newBid.BidListId)).ReturnsAsync((Bid)null!);
            _mockService.Setup(service => service.CreateBid(newBid)).ReturnsAsync(Result.Success);
            controller = new BidController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateBid(newBid);
            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task UpdateBid_NullInput_ShouldReturnBadRequest()
        {
            // Arrange
            Bid updateBid = (Bid)null!;
            controller = new BidController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateBid(1, updateBid);

            // Assert
            var createdResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, createdResult.StatusCode);
        }

        [Fact]
        public async Task UpdateBid_NotFound_ShouldReturnNotFound()
        {
            // Arrange
            Bid updateBid = new()
            {
                BidListId = 1,
                Account = "Test",
                Type = "Test Type",
                BidQuantity = 1,
                AskQuantity = 1,
                BidAmount = 12,
                Ask = 1
            };
            _mockService.Setup(service => service.GetBid(updateBid.BidListId)).ReturnsAsync((Bid)null!);
            _mockService.Setup(service => service.UpdateBid(updateBid.BidListId, updateBid)).ThrowsAsync(new KeyNotFoundException());
            controller = new BidController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateBid(updateBid.BidListId, updateBid);

            // Assert
            var createdResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, createdResult.StatusCode);
        }

        [Fact]
        public async Task UpdateBid_IdMismatch_ShouldReturnBadRequest()
        {
            Bid updateBid = new()
            {
                BidListId = 1,
                Account = "Test",
                Type = "Test Type",
                BidQuantity = 1,
                AskQuantity = 1,
                BidAmount = 12,
                Ask = 1
            };

            // Arrange -- no setup required because controller handles this logic and does not call the service
            controller = new BidController(_mockService.Object, mockLogger.Object);

            // Act - gave value of 1 for id because it does not matter - regardless of id value, method will produce error
            // if updated user is null
            var result = await controller.UpdateBid(2, updateBid);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);

        }

        [Fact]
        public async Task UpdateBid_Invalid_ShouldReturnValidationError()
        {
            // Arrange
            Bid invalidBid = new()
            {
                BidListId = 1,
                Account = "Test",
                Type = "Test Type",
                BidQuantity = -1,
                AskQuantity = 1,
                BidAmount = 12,
                Ask = 1
            };

            _mockService.Setup(service => service.GetBid(invalidBid.BidListId)).ReturnsAsync(invalidBid);
            _mockService.Setup(service => service.UpdateBid(invalidBid.BidListId, invalidBid)).ReturnsAsync(Result.Failure(
                    new Error("bid.BidQuantityNegative", "Bid Quantity cannot be negative.")));
            controller = new BidController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateBid(invalidBid.BidListId, invalidBid);

            // Assert
            var updatedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, updatedResult.StatusCode);

        }

        [Fact]
        public async Task UpdateBid_Valid_ShouldReturnSuccess()
        {
            // Arrange
            Bid updateBid = new()
            {
                BidListId = 1,
                Account = "Test",
                Type = "Test Type",
                BidQuantity = 1,
                AskQuantity = 1,
                BidAmount = 12,
                Ask = 1
            };
            _mockService.Setup(service => service.GetBid(updateBid.BidListId)).ReturnsAsync(updateBid);
            _mockService.Setup(service => service.UpdateBid(updateBid.BidListId, updateBid)).ReturnsAsync(Result.Success);
            controller = new BidController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.UpdateBid(updateBid.BidListId, updateBid);

            // Assert
            var updatedResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, updatedResult.StatusCode);
        }

        [Fact]
        public async Task DeleteBid_NotFound_ShouldReturnNotFound()
        {
            // Arrange

            _mockService.Setup(service => service.DeleteBid(3)).ThrowsAsync(new KeyNotFoundException());
            controller = new BidController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteBid(3);

            // Assert
            var deletedResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, deletedResult.StatusCode);
        }

        [Fact]
        public async Task DeleteBid_Valid_ShouldReturnSuccess()
        {
            // Arrange
            Bid deleteBid = new()
            {
                BidListId = 1,
                Account = "Test",
                Type = "Test Type",
                BidQuantity = 1,
                AskQuantity = 1,
                BidAmount = 12,
                Ask = 1
            };

            _mockService.Setup(service => service.GetBid(deleteBid.BidListId)).ReturnsAsync(deleteBid);
            _mockService.Setup(service => service.DeleteBid(deleteBid.BidListId)).ReturnsAsync(1);
            controller = new BidController(_mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteBid(deleteBid.BidListId);

            // Assert
            var deletedResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, deletedResult.StatusCode);
        }
    }
}