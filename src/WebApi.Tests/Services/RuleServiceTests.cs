using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Dot.Net.WebApi.Services;

namespace Dot.Net.Webapi.Tests
{
    public class RuleServiceTests
    {

        private readonly Mock<IRepository<Rule>> _mockRepository;
        public RuleService? RuleService;
        public Rule[] mockRules;

        public RuleServiceTests()
        {
            _mockRepository = new Mock<IRepository<Rule>>();
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
        }

        // Test Get All method

        [Fact]
        public async Task GetAll_Nonempty_ShouldReturnArray()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(mockRules);
            RuleService = new RuleService(_mockRepository.Object);

            // Act
            var getAllResult = await RuleService.GetAllRules();

            // Assert
            Assert.NotEmpty(getAllResult);
            Assert.IsType<Rule[]>(getAllResult);
        }

        [Fact]
        public async Task GetAll_Empty_ShouldReturnEmptyArray()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(Array.Empty<Rule>());
            RuleService = new RuleService(_mockRepository.Object);

            // Act
            var getAllResult = await RuleService.GetAllRules();

            // Assert
            Assert.Empty(getAllResult);
            Assert.IsType<Rule[]>(getAllResult);
        }

        // Test GetRuleByName method

        [Fact]
        public async Task GetRuleByName_ShouldReturnRule()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(mockRules[0]);
            RuleService = new RuleService(_mockRepository.Object);

            // Act
            var getResult = await RuleService.GetRule(1);

            // Assert
            Assert.Equal("Test", getResult.Name);
            Assert.IsType<Rule>(getResult);
        }

        [Fact]
        public async Task GetRuleById_NotFound_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(4)).ReturnsAsync((Rule)null!);
            RuleService = new RuleService(_mockRepository.Object);

            // Act
            var getResult = await RuleService.GetRule(4);

            // Assert
            Assert.Null(getResult);
        }

        // Test Create Rule method

        [Fact]
        public async Task CreateRule_Valid_ShouldReturnSuccess()
        {
            var newRule = new Rule
            {
                Id = 1,
                Name = "Test",
                Description = "test",
                Json = "{'object': 'value'}",
                Template = null,
                SqlStr = null,
                SqlPart = null
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(newRule.Id)).ReturnsAsync((Rule)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            RuleService = new RuleService(_mockRepository.Object);

            // Act
            var result = await RuleService.CreateRule(newRule);

            // Assert
            Assert.IsType<Result>(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateRule_NotFound_ShouldReturnException()
        {
            var newRule = new Rule
            {
                Id = 1,
                Name = "Test",
                Description = "test",
                Json = "{'object': 'value'}",
                Template = null,
                SqlStr = null,
                SqlPart = null
            };

            // Arrange
            _mockRepository.Setup(repo => repo.GetById(newRule.Id)).ReturnsAsync((Rule)null!);
            RuleService = new RuleService(_mockRepository.Object);

            // Act and Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await RuleService.UpdateRule(newRule.Id, newRule));
        }

        [Fact]
        public async Task UpdateRule_Valid_ShouldReturnSuccess()
        {
            var existingRule = mockRules[0];
            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(existingRule);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            RuleService = new RuleService(_mockRepository.Object);

            // Act
            var result = await RuleService.UpdateRule(1, existingRule);

            // Assert
            Assert.IsType<Result>(result);
            Assert.True(result.IsSuccess);
        }

        // Test DeleteRule method
        [Fact]
        public async Task DeleteRule_NotFound_ShouldReturnException()
        {

            // Arrange
            _mockRepository.Setup(repo => repo.GetById(3)).ReturnsAsync((Rule)null!);
            RuleService = new RuleService(_mockRepository.Object);

            // Act and assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await RuleService.DeleteRule(3));
        }

        [Fact]
        public async Task DeleteRule_Valid_ShouldReturnSuccess()
        {
            var existingRule = mockRules[0];
            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(existingRule);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            RuleService = new RuleService(_mockRepository.Object);

            // Act
            var result = await RuleService.DeleteRule(1);

            // Assert
            Assert.IsType<int>(result);
            Assert.Equal(1, result);
        }

        // Test validation: 4 for create method, 1 for update method

        [Fact]
        public async Task CreateRule_Invalid_ShouldReturnBuyQuantityNegativeError()
        {
            var newRule = new Rule
            {
                Id = 1,
                Name = null,
                Description = "test",
                Json = "{'object': 'value'}",
                Template = null,
                SqlStr = null,
                SqlPart = null
            };

            // Arrange 
            _mockRepository.Setup(repo => repo.GetById(newRule.Id)).ReturnsAsync((Rule)null!);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(default)).ReturnsAsync(1);
            RuleService = new RuleService(_mockRepository.Object);

            // Act
            var result = await RuleService.CreateRule(newRule);

            // Assert - should give buy quantity negative error
            Assert.IsType<Result>(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Rule.NameRequired", result.Error.Code);
        }
    }
}