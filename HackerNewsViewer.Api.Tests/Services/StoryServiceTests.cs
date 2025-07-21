using HackerNewsViewer.Clients;
using HackerNewsViewer.Models;
using HackerNewsViewer.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace HackerNewsViewer.Api.Tests.Services
{
    public class StoryServiceTests
    {
        private readonly Mock<IHackerNewsApiClient> _mockApiClient;
        private readonly IMemoryCache _memoryCache;
        private readonly StoryService _storyService;
        public StoryServiceTests()
        {
            _mockApiClient = new Mock<IHackerNewsApiClient>();
            // In-memory cache for testing
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _storyService = new StoryService(_mockApiClient.Object, _memoryCache);

            // Setup common mock behavior for story details
            _mockApiClient.Setup(x => x.GetStoryDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => new HackernewsStory { Id = id, Title = $"Story {id}", Url = $"http://link.com/{id}", Type = "story" });
        }
        [Fact]
        public async Task GetNewestStoriesAsync_ReturnsPaginatedStories()
        {
            // Arrange
            _mockApiClient.Setup(x => x.GetNewStoryIdsAsync())
                .ReturnsAsync(Enumerable.Range(1, 100).ToList()); // 100 fake IDs

            // Act
            var result = await _storyService.GetNewestStoriesAsync(null, 1, 10); // Page 1, size 10

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Count());
            Assert.Equal("Story 1", result.First().Title);
            Assert.Equal("Story 10", result.Last().Title);
        }

        [Fact]
        public async Task GetNewestStoriesAsync_AppliesSearchTerm()
        {
            // Arrange
            _mockApiClient.Setup(x => x.GetNewStoryIdsAsync())
                .ReturnsAsync(new List<int> { 1, 2, 3 });
            _mockApiClient.Setup(x => x.GetStoryDetailsAsync(1))
                .ReturnsAsync(new HackernewsStory { Id = 1, Title = "Angular Story", Type = "story", Url = "test" });
            _mockApiClient.Setup(x => x.GetStoryDetailsAsync(2))
                .ReturnsAsync(new HackernewsStory { Id = 2, Title = "DotNet Core Fun", Type = "story", Url = "test" });
            _mockApiClient.Setup(x => x.GetStoryDetailsAsync(3))
                .ReturnsAsync(new HackernewsStory { Id = 3, Title = "Hacker News", Type = "story", Url = "test" });

            // Act
            var result = await _storyService.GetNewestStoriesAsync("angular", 1, 10);

            // Assert
            Assert.Single(result);
            Assert.Equal("Angular Story", result.First().Title);
        }
    }
}
