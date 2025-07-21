using System.Net;
using System.Text;
using Xunit;
using Moq;
using Moq.Protected;
using HackerNewsViewer.Clients;


namespace HackerNewsViewer.Api.Tests.Clients
{
    public class HackerNewsApiClientTests
    {
        [Fact]
        public async Task GetNewStoryIdsAsync_ReturnsIds()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var jsonResponse = "[1, 2, 3]";
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                });
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var client = new HackerNewsApiClient(httpClient);

            // Act
            var result = await client.GetNewStoryIdsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(1, result);
        }
    }
}
