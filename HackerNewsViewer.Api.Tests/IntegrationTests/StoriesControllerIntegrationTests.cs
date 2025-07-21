using HackerNewsViewer.Clients;
using HackerNewsViewer.Models;
using Microsoft.AspNetCore.Mvc.Testing; // Add this using directive
//using HackerNewsViewer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json; // Or System.Text.Json
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HackerNewsViewer.Api.Tests.IntegrationTests
{
    public class StoriesControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public StoriesControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Replace the real IHackerNewsApiClient with a mock for integration tests
                    var hackerNewsApiClientDescriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(IHackerNewsApiClient));
                    if (hackerNewsApiClientDescriptor != null)
                    {
                        services.Remove(hackerNewsApiClientDescriptor);
                    }

                    var mockHnApiClient = new Mock<IHackerNewsApiClient>();
                    mockHnApiClient.Setup(x => x.GetNewStoryIdsAsync())
                        .ReturnsAsync(Enumerable.Range(1, 25).ToList()); // Simulate 25 IDs
                    mockHnApiClient.Setup(x => x.GetStoryDetailsAsync(It.IsAny<int>()))
                        .ReturnsAsync((int id) => new HackernewsStory { Id = id, Title = $"Test Story {id}", Url = $"http://test.com/{id}", Type = "story" });

                    services.AddSingleton(mockHnApiClient.Object);
                });
            });
        }

        [Fact]
        public async Task GetNewestStories_ReturnsOkResultWithStories()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/stories/newest?pageNumber=1&pageSize=5");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var content = await response.Content.ReadAsStringAsync();
            var stories = JsonConvert.DeserializeObject<List<StoryDto>>(content);

            Assert.NotNull(stories);
            Assert.Equal(5, stories.Count);
            Assert.Equal("Test Story 1", stories.First().Title);
        }
    }
    // Change the accessibility of Program from internal to public to match the usage in the public constructor parameter

    public class Program
    {
    }
}
