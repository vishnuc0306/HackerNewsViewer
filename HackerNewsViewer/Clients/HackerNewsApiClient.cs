using HackerNewsViewer.Models;

namespace HackerNewsViewer.Clients
{
    public class HackerNewsApiClient : IHackerNewsApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://hacker-news.firebaseio.com/v0/";
        public HackerNewsApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<int>> GetNewStoryIdsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<int>>($"{BaseUrl}newstories.json");
            return response ?? new List<int>();
        }
        public async Task<HackernewsStory> GetStoryDetailsAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<HackernewsStory>($"{BaseUrl}item/{id}.json");
            return response;
        }
    }
}
